using UnityEngine;

namespace Appcoins.Purchasing
{
    public class BDSVersionReporter : VersionReporter{
        public override string GetPluginVersionStr() {
            return "1.0.1";
        }
   
        public override string GetPluginName() {
            return "BDS Unity Plugin";
        }

        public override string GetPluginPackageName() {
            return "com.appcoins.untityplugin";
        }
    }

}
