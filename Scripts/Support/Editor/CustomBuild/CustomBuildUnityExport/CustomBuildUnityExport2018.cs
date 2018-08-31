using UnityEditor;

public class CustomBuildUnityExport2018 : CustomBuildUnityExport
{
    protected BuildTargetGroup buildTargetGroup;

    public CustomBuildUnityExport2018(BuildTarget bT, BuildOptions bO, 
                                      BuildTargetGroup bG, 
                                      ICustomBuildTarget target) 
        : base(bT, bO, target)
    {
        buildTargetGroup = bG;
    }

    internal override void UnityExport(BuildStage stage, string[] scenesPath,
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