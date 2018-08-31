using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomBuildUnityExport5_6 : CustomBuildUnityExport
{
    private string rightDllLoc;
    private string tempDllLoc;

    public CustomBuildUnityExport5_6(BuildTarget bT, BuildOptions bO, 
                                     ICustomBuildTarget target) :
        base(bT, bO, target)
    {
        rightDllLoc = Application.dataPath + "/AppcoinsUnity/Scripts/" +
                                 "AppCoinsUnityPluginTests5_6.dll";

        tempDllLoc = Directory.GetParent(Application.dataPath).FullName +
                              "/AppCoinsUnityPluginTests5_6.dll";
    }

    internal override void UnityExport(BuildStage stage, string[] scenesPath,
                                       out string projPath)
    {
        platformTarget.RunAditionalSteps();

        string containerPath = SelectProjectPath(PlayerSettings.productName);
        projPath = containerPath + "/" + PlayerSettings.productName;

        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Android, BuildTarget.Android);

        // Remove AppcoinsUnityTests.dll from the project
        File.Move(rightDllLoc, tempDllLoc);
        AssetDatabase.Refresh();

        string s = BuildPipeline.BuildPlayer(scenesPath, containerPath, 
                                             buildTarget,
                                             buildOptions);

        // Add AppcoinsUnityTests.dll to the project
        File.Move(tempDllLoc, rightDllLoc);
        AssetDatabase.Refresh();

        // If Export failed 's' contains something.
        if (!s.Equals(""))
        {
            throw new ExportProjectFailedException();
        }
    }
}