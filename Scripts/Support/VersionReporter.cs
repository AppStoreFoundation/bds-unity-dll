using UnityEngine;

namespace Appcoins.Purchasing
{
  public class VersionReporter : MonoBehaviour{

      public void LogVersionDetails() {
            Debug.Log("Using " + GetPluginName() + "\nplugin packageName: " + GetPluginPackageName() + "\nversion: " + GetPluginVersionStr() + "\nverCode: " + GetPluginVerCodeStr() + "\non Unity " + GetUnityVersionStr());
      }

      public string GetUnityVersionStr() {
        return Application.unityVersion;
      }

        public virtual string GetPluginVersionStr() {
        return "OVERRIDE ME";
      }

        public virtual string GetPluginVerCodeStr()
        {
            return "OVERRIDE ME";
        }

      public virtual string GetPluginName() {
        return "OVERRIDE ME";
      }

      public virtual string GetPluginPackageName() {
        return "OVERRIDE ME";
      }
  }

}
