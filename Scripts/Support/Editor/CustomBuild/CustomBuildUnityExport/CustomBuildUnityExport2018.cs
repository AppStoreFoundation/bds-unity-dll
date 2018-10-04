using UnityEditor;

public class CustomBuildUnityExport2018 : CustomBuildUnityExport
{
    public CustomBuildUnityExport2018(BuildTarget bT, BuildOptions bO, 
                                      BuildTargetGroup bG, 
                                      ICustomBuildTarget target) 
        : base(bT, bG, bO, target) {}

    public override void UnityExport(BuildStage stage, string[] scenesPath,
                                       out string projPath)
    {
        platformTarget.RunAditionalSteps();
        string containerPath = SelectProjectPath(PlayerSettings.productName);
        projPath = containerPath + "/" + PlayerSettings.productName;

        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup,
                                                        buildTarget);

        UnityEditor.Build.Reporting.BuildReport error = 
            BuildPipeline.BuildPlayer(scenesPath, containerPath, buildTarget, 
                                      buildOptions);

        // Check if export failed.
        bool fail = (
            error.summary.result == 
                UnityEditor.Build.Reporting.BuildResult.Failed ||
            error.summary.result == 
                UnityEditor.Build.Reporting.BuildResult.Cancelled
        );

        if (fail)
        {
            throw new ExportProjectFailedException();
        }
    }
}