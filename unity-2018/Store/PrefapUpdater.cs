using System;
using UnityEngine;
using Appcoins.Purchasing;


[ExecuteInEditMode]
public class PrefapUpdater : MonoBehaviour
{
    private AppcoinsPurchasing appcoinsPurchasing;
    private string newPrefabName;
    private bool newUseAdsSDK;
    private bool newUseMainNet;


    void Awake()
    {
        appcoinsPurchasing = GetComponent<AppcoinsPurchasing>();
        newPrefabName = gameObject.name;
        Debug.Log("awakee");
        newUseAdsSDK = appcoinsPurchasing.UsesAdsSDK();
        newUseMainNet = appcoinsPurchasing.UsesMainNet();
    }

    void Update()
    {
        bool setupValue = false;
        if (!newPrefabName.Equals(gameObject.name))
        {
            Debug.Log("Name has been changed");
            newPrefabName = gameObject.name;
            setupValue = true;
        }
        else
        {
            Debug.Log("Name still the same");
        }

        if(!appcoinsPurchasing.UsesMainNet() || !appcoinsPurchasing.UsesAdsSDK())
        {
            setupValue = true;
        }
        Debug.Log("updateee");
        if(setupValue.Equals(true))
        {
            Debug.Log("Setup function deployed");
        }
    }
}


