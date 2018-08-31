using UnityEditor;

public abstract class CustomBuildErrorTitles
{
    string[] genericErrorTitles = {
        "Export Unity Project: ",
        "Build Exported Project: ",
        "Install .apk to device: ",
        "Run .apk in the device: "
    };

    public virtual void SetErrorTitles()
    {
        for (int i = 0; i < genericErrorTitles.Length; i++)
        {
            EditorPrefs.SetString("appcoins_error_title_" + i.ToString(),
                                      genericErrorTitles[i]);
        }
    }
}