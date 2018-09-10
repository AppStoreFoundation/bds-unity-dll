using UnityEditor;

public class CustomBuildUnityExport2017 : CustomBuildUnityExport
{
    protected BuildTargetGroup buildTargetGroup;

    public CustomBuildUnityExport2017(BuildTarget bT, BuildOptions bO,
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

        string s = BuildPipeline.BuildPlayer(
            scenesPath, containerPath, buildTarget, buildOptions);

        // If Export is done succesfully s is: "".
        if (!s.Equals(""))
        {
            throw new ExportProjectFailedException();
        }
    }
}