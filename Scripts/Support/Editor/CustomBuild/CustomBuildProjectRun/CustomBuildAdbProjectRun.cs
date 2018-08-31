using UnityEditor;

public class CustomBuildAdbProjectRun : CustomBuildProjectRun
{
    private Terminal terminal;

    public CustomBuildAdbProjectRun()
    {
        terminal = Tools.GetTerminalByOS();
    }

    private void GetAdbRunArgs(out string adbOptions, out string adbArgs)
    {
        adbOptions = "shell am start -n";
        adbArgs = EditorPrefs.GetString("appcoins_main_activity_path", "");
    }

    internal override void RunProject(BuildStage stage, string projPath)
    {

        string command = Tools.FixAppPath(
            EditorPrefs.GetString("appcoins_adb_path", ""),
            "adb");

        GetAdbRunArgs(out string adbOptions, out string adbArgs);
        terminal.RunCommand(stage, command, adbOptions, adbArgs, projPath, 
                            false);
    }
}