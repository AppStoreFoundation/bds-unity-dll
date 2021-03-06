﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Appcoins.Purchasing;
using UnityEngine.Events;

public class PurchaseEvent : UnityEvent<AppcoinsProduct> {
    
}

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
//[RequireComponent(typeof(AppcoinsPurchasing))]
public class Purchaser : MonoBehaviour, IAppcoinsStoreListener
{
    public UnityEvent onInitializeSuccess;
    public UnityEvent onInitializeFailed;

    public PurchaseEvent onPurchaseSuccess;
    public PurchaseEvent onPurchaseFailed;

    [SerializeField]
    private Text _statusText;

    private static AppcoinsStoreController m_StoreController;          // The AppCoins Purchasing system.

    private AppcoinsPurchasing _appcoinsPurchasing;
    private AppcoinsConfigurationBuilder _builder;

    private string _pendingPurchaseSkuID;

    private VersionReporter _versionReporter;
    private EventLogger _logger;

    public static Purchaser Create() {
        GameObject appcoinsPurchasing = new GameObject();
        appcoinsPurchasing.name = "AppcoinsPurchasing";
        return appcoinsPurchasing.AddComponent<Purchaser>();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        onInitializeSuccess = new UnityEvent();
        onInitializeFailed = new UnityEvent();

        onPurchaseSuccess = new PurchaseEvent();
        onPurchaseFailed = new PurchaseEvent();

        _builder = new AppcoinsConfigurationBuilder();

        _appcoinsPurchasing = gameObject.AddComponent<AppcoinsPurchasing>();

        _versionReporter = gameObject.AddComponent<BDSVersionReporter>();
        if (_versionReporter == null)
        {
            //Debug.LogError("Failed initializing! Plugin prefab is missing BDS VersionReporter. Please use the unmodified version of the prefab.");
            Debug.LogError("Failed initializing! Plugin prefab is missing BDS VersionReporter. Please use the unmodified version of the prefab.");
            return;
        }
    }

    public void AddProduct(string skuID, AppcoinsProductType type)
    {
        _builder.AddProduct(skuID, type);
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        _versionReporter.LogVersionDetails();

        _logger = gameObject.AddComponent<EventLogger>();

        SetStatus("UnityPurchasing initializing.");
        if (Application.isEditor)
        {
            OnInitialized(null);
        }
        else
        {
            _appcoinsPurchasing.Initialize(this, _builder);
        }
    }

    public void SetStatusLabel(Text statusLabel)
    {
        _statusText = statusLabel;
    }

    void SetStatus(string status)
    {
        if (_statusText != null)
            _statusText.text = status;
        Debug.Log(status);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null;
    }

    public void BuyProductID(string productId)
    {
        if (Application.isEditor)
        {
            AppcoinsProduct product = new AppcoinsProduct();
            product.skuID = productId;
            ProcessPurchase(product);

        }
        else
        {
            //Fire event
            //Remove APPC from the price string
            string priceStr = _appcoinsPurchasing.GetAPPCPriceStringForSKU(productId).Replace(" APPC", "");
            FireBuyIntentEvent(productId, priceStr);

            //Check if wallet is installed
            if (!_appcoinsPurchasing.HasWalletInstalled())
            {
                SetStatus("BuyProductID: FAIL. Not purchasing product, no wallet app found on device!");
                _appcoinsPurchasing.PromptWalletInstall();
                _pendingPurchaseSkuID = productId;

                m_StoreController = null; //To force reinitialization
                return;
            }

            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing
                // system's products collection.
                AppcoinsProduct product = m_StoreController.products.WithID(productId);

                //If the look up found a product for this device's store and that product is ready to be sold ...
                if (product != null)
                {
                    if (product.productType == AppcoinsProductType.NonConsumable && OwnsProduct(productId))
                    {
                        OnPurchaseFailed(product, AppcoinsPurchaseFailureReason.DuplicateTransaction);
                        SetStatus("BuyProductID: FAIL. Not purchasing product, non-consumable is already owned!");
                        return;
                    }

                    SetStatus(string.Format("Purchasing product asychronously: '{0}'", product.skuID));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed
                    // asynchronously.


                    /* TODO: for security, generate your payload here for verification. See the comments on
                     *        verifyDeveloperPayload() for more info. Since this is a SAMPLE, we just use
                     *        an empty string, but on a production app you should carefully generate this.
                     * TODO: On this payload the developer's wallet address must be added, or the purchase does NOT work.
                     */
                    string payload = "";
                    m_StoreController.InitiatePurchase(product, payload);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation
                    SetStatus("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or
                // retrying initiailization.
                SetStatus("BuyProductID FAIL. Not initialized. Trying to initialize now");
                InitializePurchasing();
            }
        }

    }

    bool OwnsProduct(string skuID)
    {
        return _appcoinsPurchasing.OwnsProduct(skuID);
    }

    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
    }

    void FireBuyIntentEvent(string sku, string value)
    {
        _logger.LogBuyIntent(
            _versionReporter.GetPluginVerCodeStr(),
            _versionReporter.GetPluginPackageName(),
            _appcoinsPurchasing.GetBundleIdentifier(),
            sku,
            value,
            _appcoinsPurchasing.HasWalletInstalled(),
            _versionReporter.GetUnityVersionStr());
    }

    //
    // --- IAppcoinsStoreListener
    //

    public void OnInitialized(AppcoinsStoreController controller)
    {
        onInitializeSuccess.Invoke();

        // Purchasing has succeeded initializing. Collect our Purchasing references.
        SetStatus("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;

        if (_pendingPurchaseSkuID != null && !_pendingPurchaseSkuID.Equals(""))
        {
            SetStatus("OnInitialized: PASS! Resuming pending purchase of: " + _pendingPurchaseSkuID);
            BuyProductID(_pendingPurchaseSkuID);
            _pendingPurchaseSkuID = "";
        }
    }


    public void OnInitializeFailed(AppcoinsInitializationFailureReason error)
    {
        onInitializeFailed.Invoke();

        SetStatus("OnInitialized: FAILED: " + error);
    }


    public AppcoinsPurchaseProcessingResult ProcessPurchase(AppcoinsProduct p)
    {
        SetStatus("Processed purchase " + p.skuID);

        onPurchaseSuccess.Invoke(p);

        return AppcoinsPurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(AppcoinsProduct product, AppcoinsPurchaseFailureReason failureReason)
    {
        onPurchaseFailed.Invoke(product);

        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing
        // this reason with the user to guide their troubleshooting actions.
        SetStatus(string.Format("OnPurchaseFailed: FAIL.\nProduct: '{0}',\nPurchaseFailureReason: {1}", (product != null ? product.skuID : "none"), failureReason));
    }

    #region AppcoinsPurchasing facilitators
    public void SetupCustomValidator(IPayloadValidator customValidator) {
        if (_appcoinsPurchasing == null)
        {
            Debug.LogError("ERROR! No appcoinsPurchasing object when trying to setup custom validator");
            return;
        }

        _appcoinsPurchasing.SetupCustomValidator(customValidator);
    }
        

    public string GetAPPCPriceStringForSKU(string skuID) {
        if (_appcoinsPurchasing == null) {
            Debug.LogError("ERROR! No appcoinsPurchasing object when trying to get APPC value");
            return "ERROR";
        }
            
        return _appcoinsPurchasing.GetAPPCPriceStringForSKU(skuID);
    }

    public string GetFiatCurrencyCodeForSKU(string skuID)
    {
        if (_appcoinsPurchasing == null)
        {
            Debug.LogError("ERROR! No appcoinsPurchasing object when trying to get Currency code value");
            return "ERROR";
        }

        return _appcoinsPurchasing.GetFiatCurrencyCodeForSKU(skuID);
    }

    public string GetFiatPriceStringForSKU(string skuID)
    {
        if (_appcoinsPurchasing == null)
        {
            Debug.LogError("ERROR! No appcoinsPurchasing object when trying to get fiat value");
            return "ERROR";
        }

        return _appcoinsPurchasing.GetFiatPriceStringForSKU(skuID);
    }


#endregion
}
