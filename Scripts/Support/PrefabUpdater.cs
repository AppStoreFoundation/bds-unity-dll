using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

//This class makes sure that the prefab changes are transmited to mainTemplate.gradle before the build actually starts
class PrefabUpdater : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Detected build is about to start... making sure all values are up to date...");
        CustomBuildMenuItem.SetupBuild();
    }
}