using UnityEditor;

public class CustomBuildAndroidErrorTitles : CustomBuildErrorTitles
{
    private string[] customErrorTitles = {
        "Export Unity Project: ",
        "(GRADLE) Build Exported Project: ",
        "(ADB) Install .apk to device: ",
        "(ADB) Run .apk in the device: "
    };

    public override void SetErrorTitles()
    {
        for (int i = 0; i < customErrorTitles.Length; i++)
        {
            EditorPrefs.SetString("appcoins_error_title_" + i.ToString(),
                                  customErrorTitles[i]);
        }
    }
}