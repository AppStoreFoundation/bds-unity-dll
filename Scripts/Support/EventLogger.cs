using UnityEngine;

using System.Collections.Generic;
using System.Collections;

namespace Appcoins.Purchasing
{
    public class EventLogger : MonoBehaviour
    {
        public void LogBuyIntent(string verCode,
                                        string pluginPackage,
                                        string appPackage,
                                        string sku,
                                        string value,
                                        bool hasWallet,
                                        string unityVersion)
        {
            string url = "http://ws75.aptoide.com/api/7/user/addEvent/action=CLICK/context=UNITY/name=PURCHASE_INTENT";
            string json = "{ \"aptoide_vercode\": " + verCode + ", \"aptoide_package\": \"" + pluginPackage + "\", \"data\": { \"purchase\": { \"package_name\":\"" + appPackage + "\", \"sku\":\"" + sku + "\", \"value\": " + value + "}, \"wallet_installed\": " + hasWallet.ToString().ToLower() + ", \"unity_version\": \"" + unityVersion + "\"} }";
      
            PostDataToURLWithHeadersDict(url, json);
        }

        public void PostDataToURLWithHeadersDict(string fullURL, string json)
        {
            Debug.Log("going to post to:\nurl: " + fullURL + "\njson: " + json);

            WWWForm form = new WWWForm();
            byte[] formData = null;

            Dictionary<string, string> headers = form.headers;
            headers["content-type"] = "application/json";

            byte[] latestFormData;
            WWW request;
            {
                formData = System.Text.Encoding.UTF8.GetBytes(json);
                request = new WWW(fullURL, formData, headers);
                latestFormData = formData;
            }

            StartCoroutine(WaitForRequest(request));
        }

        IEnumerator WaitForRequest(WWW www)
        {
            yield return www;
            if (www.error == null)
            {
                //Print server response
                Debug.Log("Server response: " + www.text);
            }
            else
            {
                //Something goes wrong, print the error response
                Debug.LogError("ERROR: " + www.text);
            }
        }
    }
}