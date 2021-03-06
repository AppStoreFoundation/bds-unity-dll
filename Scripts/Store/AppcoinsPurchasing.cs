﻿using System;
using UnityEngine;

namespace Appcoins.Purchasing
{
    public enum AppcoinsPurchaseProcessingResult
    {
        Complete,
        Pending
    }

    public enum AppcoinsInitializationFailureReason
    {
        PurchasingUnavailable,
        NoProductsAvailable,
        AppNotKnown,
        WalletNotInstalled,
        NetworkNotAvailable
    }

    public enum AppcoinsPurchaseFailureReason
    {
        PurchasingUnavailable,
        ExistingPurchasePending,
        ProductUnavailable,
        SignatureInvalid,
        UserCancelled,
        PaymentDeclined,
        DuplicateTransaction,
        Unknown
    }

    public interface IPayloadValidator
    {
        bool IsValidPayload(string payload);
    }

    public class AppcoinsPurchasing : MonoBehaviour
    {
        public const string APPCOINS_PREFAB = "AppcoinsPurchasing";

        [SerializeField]
        private string _developerWalletAddress;

        [SerializeField]
        private string _developerBDSPublicKey;

        [SerializeField]
        private bool _shouldLog;

        [SerializeField]
        private bool _useAdsSDK = true;

        //[SerializeField]
        private bool _useMainNet = true;

        private AndroidJavaClass _class;
        private AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

        private const string JAVA_CLASS_NAME = "com.aptoide.iabexample.UnityAppcoins";

        // Only class that can comunicate with AppcoinsPurchasing
        private AppcoinsStoreController _controller;

        // AppcoinsStore listener (call specific methods regarding some output purchase)
        private IAppcoinsStoreListener _listener;

        // Store to be able to provide more context to purchase failed errors.
        // We can only get one argument from java function calls so we return the error instead of the skuID
        private AppcoinsProduct _currentPurchaseProduct;

        private IPayloadValidator _customPayloadValidator;

        //  Create an instance of this class. Add an AppcoinsStore listener and
        //  get products definitions from ConfigurationBuilder
        public void Initialize(IAppcoinsStoreListener listener, AppcoinsConfigurationBuilder builder)
        {
            try
            {
                _listener = listener;
                _controller = new AppcoinsStoreController(this, builder);

                CreateAndSetupJavaBind();
            }
            // Catch specific errors and then call OnInitializesFail
            catch (System.Exception e)
            {
                Debug.LogError("Failed with exception " + e);
                OnInitializeFail(e.ToString());
            }
        }

        private void CreateAndSetupJavaBind()
        {
            //get refference to java class
            _class = new AndroidJavaClass(JAVA_CLASS_NAME);

            //Setup sdk
            _class.CallStatic("setLogging", _shouldLog);
            _class.CallStatic("setUseMainNet", _useMainNet);
            _class.CallStatic("setUseAdsSDK", _useAdsSDK);


            Debug.Log("the bundle is " + GetBundleIdentifier());

            //start sdk
            _class.CallStatic("start");
        }

        // After successful initialization create controler and add listener
        public void OnInitializeSuccess()
        {
            _listener.OnInitialized(_controller);
        }

        public bool HasWalletInstalled()
        {
            return _class.CallStatic<bool>("hasSpecificWalletInstalled");
        }

        public void PromptWalletInstall()
        {
            _class.CallStatic("promptWalletInstall");
        }

        public void OnInitializeFail(string error)
        {
            Debug.Log("Called OnInitializeFail with reaason " + error);

            AppcoinsInitializationFailureReason reason = InitializationFailureReasoFromString(error);

            _listener.OnInitializeFailed(reason);
        }

        AppcoinsInitializationFailureReason InitializationFailureReasoFromString(string errorStr)
        {
            AppcoinsInitializationFailureReason reason = AppcoinsInitializationFailureReason.PurchasingUnavailable;

            if (errorStr.Contains("Billing service unavailable on device"))
            {
                reason = AppcoinsInitializationFailureReason.WalletNotInstalled;
            }
            else if (errorStr.Contains("Error checking for billing v3 support."))
            {
                reason = AppcoinsInitializationFailureReason.AppNotKnown;
            }
            else if (errorStr.Contains("No Network Available."))
            {
                reason = AppcoinsInitializationFailureReason.NetworkNotAvailable;
            }

            return reason;
        }

        public string GetAPPCPriceStringForSKU(string skuID)
        {
            Debug.Log("Getting appc price for sku " + skuID);
            string price = instance.Call<string>("getAPPCPriceStringForSKU", skuID);

            Debug.Log("Price for sku " + skuID + " is " + price);
            return price;
        }

        public string GetFiatPriceStringForSKU(string skuID)
        {
            Debug.Log("Getting fiat price for sku " + skuID);
            string price = instance.Call<string>("getFiatPriceStringForSKU", skuID);


            Debug.Log("Price for sku " + skuID + " is " + price);
            return price;
        }

        public string GetFiatCurrencyCodeForSKU(string skuID)
        {
            Debug.Log("Getting fiat currency code for " + skuID);
            string code = instance.Call<string>("getFiatCurrencyCodeForSKU", skuID);


            Debug.Log("The currency code for " + skuID + " price is " + code);
            return code;
        }

        public void InitiatePurchase(AppcoinsProduct prod, string payload)
        {
            _currentPurchaseProduct = prod;
            instance.Call("makePurchase", prod.skuID, payload);
        }

        //callback on successful purchases (called by java class)
        public void OnProcessPurchase(string skuID)
        {
            Debug.Log("Purchase successful for skuid: " + skuID);
            bool success = true;
            if (success)
            {
                if (_listener == null)
                {
                    Debug.LogError("No IStoreListener set up!");
                }

                AppcoinsProduct product = _controller.products.WithID(skuID);
                _currentPurchaseProduct = product;

                switch (product.productType)
                {
                    case AppcoinsProductType.Consumable:
                        instance.Call("consumePurchase", skuID);
                        break;
                    case AppcoinsProductType.NonConsumable:
                        //Won't consume so skip right through to success
                        OnPurchaseSuccess(skuID);
                        break;
                    case AppcoinsProductType.Subscription:
                        Debug.LogError("We still don't support Subscriptions! Sorry about that...");
                        break;
                }
            }
        }

        public void OnPurchaseSuccess(string skuID)
        {
            AppcoinsProduct product = _controller.products.WithID(skuID);
            _listener.ProcessPurchase(product);

            _currentPurchaseProduct = null;
        }

        //callback on failed purchases
        public void OnPurchaseFailure(string error)
        {
            Debug.Log("Purchase failed with error " + error);
            if (_listener == null)
            {
                Debug.LogError("No IStoreListener set up!");
            }

            if (_controller == null)
            {
                Debug.LogError("No IStoreController set up!");
            }

            AppcoinsPurchaseFailureReason failureReason = PurchaseFailureReasonFromString(error);
            _listener.OnPurchaseFailed(_currentPurchaseProduct, failureReason);

            _currentPurchaseProduct = null;
        }

        AppcoinsPurchaseFailureReason PurchaseFailureReasonFromString(string error)
        {
            AppcoinsPurchaseFailureReason reason = AppcoinsPurchaseFailureReason.Unknown;

            //        String[] iab_msgs = ("0:OK/1:User Canceled/2:Unknown/"
            //+ "3:Billing Unavailable/4:Item unavailable/"
            //+ "5:Developer Error/6:Error/7:Item Already Owned/"
            //+ "8:Item not owned").split("/");
            //String[] iabhelper_msgs = ("0:OK/-1001:Remote exception during initialization/"
            //+ "-1002:Bad response received/"
            //+ "-1003:Purchase signature verification failed/"
            //+ "-1004:Send intent failed/"
            //+ "-1005:User cancelled/"
            //+ "-1006:Unknown purchase response/"
            //+ "-1007:Missing token/"
            //+ "-1008:Unknown error/"
            //+ "-1009:Subscriptions not available/"
            //+ "-1010:Invalid consumption attempt").split("/");

            if (error.IndexOf("User cancelled", StringComparison.OrdinalIgnoreCase) != -1)
            {
                reason = AppcoinsPurchaseFailureReason.UserCancelled;
            }
            else if (error.IndexOf("Unknown error", StringComparison.OrdinalIgnoreCase) != -1)
            {
                reason = AppcoinsPurchaseFailureReason.Unknown;
            }
            else if (error.IndexOf("Purchase signature verification failed", StringComparison.OrdinalIgnoreCase) != -1)
            {
                reason = AppcoinsPurchaseFailureReason.SignatureInvalid;
            }
            else if (error.IndexOf("Unable to buy item", StringComparison.OrdinalIgnoreCase) != -1)
            {
                reason = AppcoinsPurchaseFailureReason.ProductUnavailable;
            }
            else if (error.IndexOf("Unknown error", StringComparison.OrdinalIgnoreCase) != -1)
            {
                reason = AppcoinsPurchaseFailureReason.Unknown;
            }

            return reason;
        }

        public void SetupCustomValidator(IPayloadValidator customValidator)
        {
            _customPayloadValidator = customValidator;
        }

        bool IsValidPayload(string payload)
        {
            if (_customPayloadValidator != null)
            {
                return _customPayloadValidator.IsValidPayload(payload);
            }
            else
            {
                Debug.Log("Validating payload " + payload);
                return true;
            }
        }

        public void AskForPayloadValidation(string payload)
        {
            bool result = IsValidPayload(payload);

            instance.Call("setPayloadValidationStatus", result);
        }

        public bool OwnsProduct(string skuID)
        {
            return instance.Call<bool>("OwnsProduct", skuID);
        }

        public bool UsesMainNet()
        {
            return _useMainNet;
        }

        public bool UsesAdsSDK()
        {
            return _useAdsSDK;
        }

        public string GetBundleIdentifier()
        {
            return _class.CallStatic<string>("getPackageName");
        }

        public bool UsesLog(){
            return _shouldLog;
        }
    }
}
