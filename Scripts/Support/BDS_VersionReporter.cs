using UnityEngine;

namespace Appcoins.Purchasing
{
    public class BDSVersionReporter : VersionReporter{

        //Each time a new verCode is added comment the previous one with the release date   
        private int _verCode = 6; //1.0.4
        //private int _verCode = 5; //1.0.3

        public override string GetPluginVersionStr() {
            return "1.0.4";
        }

        public override string GetPluginVerCodeStr()
        {
            return _verCode.ToString();
        }
   
        public override string GetPluginName() {
            return "BDS Unity Plugin";
        }

        public override string GetPluginPackageName() {
            return "com.appcoins.unityplugin";
        }
    }

}
