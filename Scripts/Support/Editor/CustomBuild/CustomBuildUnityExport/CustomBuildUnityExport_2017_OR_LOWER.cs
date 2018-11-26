using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomBuildUnityExport2017OrLower : CustomBuildUnityExport
{
    //private string rightDllLoc;
    //private string tempDllLoc;

    public CustomBuildUnityExport2017OrLower(BuildTarget bT, 
                                             BuildTargetGroup bTG, 
                                             BuildOptions bO, 
                                             ICustomBuildTarget target) :
        base(bT, bTG, bO, target) {}

    public override void UnityExport(BuildStage stage, string[] scenesPath,
                                       out string projPath)
    {
        platformTarget.RunAditionalSteps();

        string containerPath = SelectProjectPath(PlayerSettings.productName);
        projPath = containerPath + "/" + PlayerSettings.productName;

        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Android, BuildTarget.Android);

        // Remove AppcoinsUnityTests.dll from the project
        //File.Move(rightDllLoc, tempDllLoc);
        //AssetDatabase.Refresh();

        string s = BuildPipeline.BuildPlayer(scenesPath, containerPath, 
                                             buildTarget,
                                             buildOptions).ToString();

        // Add AppcoinsUnityTests.dll to the project
        //File.Move(tempDllLoc, rightDllLoc);
        //AssetDatabase.Refresh();

        // If Export failed 's' contains something.
        if (!s.Equals(""))
        {
            throw new ExportProjectFailedException();
        }
    }
}