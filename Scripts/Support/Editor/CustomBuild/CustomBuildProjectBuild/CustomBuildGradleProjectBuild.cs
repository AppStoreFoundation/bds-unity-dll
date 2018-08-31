using UnityEditor;
using UnityEngine;

public class CustomBuildGradleProjectBuild : CustomBuildProjectBuild
{
    private string gradleMem = "1536";
    private const string gradleMemLine = "org.gradle.jvmargs=-Xmx{0}M";

    private bool gradleDebugMode = false;

    private Terminal terminal;

    public CustomBuildGradleProjectBuild()
    {
        terminal = Tools.GetTerminalByOS();
    }

    private string GetGradleArgs()
    {
        string gradleArgs = "assembleDebug";

        if (EditorPrefs.GetBool("appcoins_build_release", false))
        {
            gradleArgs = "assembleRelease";
        }

        if ((gradleDebugMode = 
             EditorPrefs.GetBool("appcoins_debug_mode", false)))
        {
            gradleArgs += " --debug";
        }

        return gradleArgs;
    }

    private void TurnGradleIntoExe(BuildStage stage, string gradlePath)
    {
        // If we're not in windows we need to make sure that the gradle file has 
        // exec permission and if not, set them
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            string chmodCmd = "chmod";

            terminal.RunCommand(stage, chmodCmd, "+x", gradlePath, ".", false);
        }
    }

    private void ChangeGradleMem(string projPath)
    {
        string buildGradlePath = projPath + "/gradle.properties";
        gradleMem = EditorPrefs.GetString("appcoins_gradle_mem", "1536");
        string[] lines = { gradleMemLine.Replace("{0}", gradleMem) };

        Tools.WriteToFile(buildGradlePath, lines);
    }

    internal override void BuildProject(BuildStage stage, string projPath)
    {
        string command = Tools.FixAppPath(
            EditorPrefs.GetString("appcoins_gradle_path", ""), 
            "gradle"
        );

        TurnGradleIntoExe(stage, command);

        string gradleArgs = GetGradleArgs();
        terminal.RunCommand(stage, command, gradleArgs, "", projPath, 
                            gradleDebugMode);

        ChangeGradleMem(projPath);
    }
}