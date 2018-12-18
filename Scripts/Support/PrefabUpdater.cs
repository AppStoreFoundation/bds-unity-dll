using UnityEditor;
using UnityEngine;

using Appcoins.Purchasing;

[ExecuteInEditMode]
public class PrefabUpdater : MonoBehaviour {

    private AppcoinsPurchasing _appcoins;
    private string _lastPrefabName;
    private bool _lastUseAdsSDK;

    void Awake()
    {
        _appcoins = GetComponent<AppcoinsPurchasing>();
        _lastPrefabName = gameObject.name;
        _lastUseAdsSDK = _appcoins.UsesAdsSDK();
    }

    //Detect if the prefab has suffered any possible change
    void Update()
    {
        bool shouldRunSetup = false;

        //Detect if the prefab name has been changed
        if (!_lastPrefabName.Equals(gameObject.name))
        {
            _lastPrefabName = gameObject.name;
            shouldRunSetup = true;

            //Notify any prefab name changes
            Debug.Log("Name has been updated");
        }

        if (!_appcoins.UsesAdsSDK().Equals(_lastUseAdsSDK))
        {
            _lastUseAdsSDK = _appcoins.UsesAdsSDK();
            shouldRunSetup = true;

            //Notify any prefab AdsSDK changes
            Debug.Log("AdsSDK has been updated");
        }

        if (shouldRunSetup.Equals(true))
        {
            //Start setup
            EditorApplication.ExecuteMenuItem("AppCoins/Setup");
            Debug.Log("Successfully applied the recent changes");
        }
    }
}
