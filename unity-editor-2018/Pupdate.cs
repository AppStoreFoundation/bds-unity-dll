using System;
using UnityEngine;
using UnityEditor;
using Appcoins.Purchasing;

namespace unityeditor2018
{

    [ExecuteInEditMode]
    public class Pupdate : MonoBehaviour
    {

        private AppcoinsPurchasing appcoins;
        private string newPrefabName;
        private bool newUseAdsSDK;

        void Awake()
        {
            appcoins = GetComponent<AppcoinsPurchasing>();
            newPrefabName = gameObject.name;
            newUseAdsSDK = appcoins.UsesAdsSDK();

            //Start Setup
            CustomBuildMenuItem.SetupBuild();

            //Awake Instant
            Debug.Log("awake");
        }

        //Detect if the prefab has suffered any possible change
        void Update()
        {
            bool setupValue = false;

            //Detect if the prefab name has been altered
            if (!newPrefabName.Equals(gameObject.name))
            {
                newPrefabName = gameObject.name;
                setupValue = true;

                //Notify any prefab name changes
                Debug.Log("Name has been changed");
            }
            else
            {
                Debug.Log("Name is still the same");
            }

            if (!appcoins.UsesAdsSDK().Equals(newUseAdsSDK))
            {
                newUseAdsSDK = appcoins.UsesAdsSDK();
                setupValue = true;

                //Notify any prefab AdsSDK changes
                Debug.Log("AdsSDK has been changed");
            }

            //Frame update
            Debug.Log("updating");

            if (setupValue.Equals(true))
            {
                //Start setup
                CustomBuildMenuItem.SetupBuild();
            }
        }
    }
}


