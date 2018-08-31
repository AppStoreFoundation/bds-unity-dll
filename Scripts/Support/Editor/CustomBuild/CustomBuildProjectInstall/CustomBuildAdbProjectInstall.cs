using UnityEditor;

public class CustomBuildAdbProjectInstall : CustomBuildProjectInstall
{
    private Terminal terminal;

    public CustomBuildAdbProjectInstall()
    {
        terminal = Tools.GetTerminalByOS();
    }

    private void GetAdbInstallArgs(out string adbOptions, out string adbArgs)
    {
        adbOptions = "-d install -r";

        if (EditorPrefs.GetBool("appcoins_build_release", false))
        {
            adbArgs = "./build/outputs/apk/release/" +
                       PlayerSettings.productName + "-release.apk";
        }

        else
        {
            adbArgs = "./build/outputs/apk/debug/" +
                       PlayerSettings.productName + "-debug.apk";
        }
    }

    internal override void InstallProject(BuildStage stage, string projPath)
    {
        string command = Tools.FixAppPath(
            EditorPrefs.GetString("appcoins_adb_path", ""), 
            "adb");

        GetAdbInstallArgs(out string adbOptions, out string adbArgs);
        terminal.RunCommand(stage, command, adbOptions, adbArgs, projPath, 
                            false);
    }
}