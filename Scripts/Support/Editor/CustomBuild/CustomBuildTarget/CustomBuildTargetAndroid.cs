using UnityEditor;
using UnityEngine;

using System;

public class CustomBuildTargetAndroid : ICustomBuildTarget
{
    private string mainTemplatePath;
    private string[] dexContainers = { "dexOptions" };
    private const string dexMainTemplateLine = "javaMaxHeapSize";
    private const string dexMainTemplateNewLine = "javaMaxHeapSize \"{0}g\"";
    private const int GbToMb = 1024;


    public CustomBuildTargetAndroid()
    {
        mainTemplatePath = Application.dataPath +
                                      "/Plugins/Android/mainTemplate.gradle";
    }

    void ICustomBuildTarget.RunAditionalSteps()
    {
        AddDexOptionsToMainTemplate();
    }

    protected void AddDexOptionsToMainTemplate()
    {
        string dexMem = EditorPrefs.GetString("appcoins_dex_mem", "1024");
        dexMem = (Int32.Parse(dexMem) / GbToMb).ToString();

        string newDexLine = dexMainTemplateNewLine.Replace("{0}", dexMem);

        Tools.ChangeLineInFile(mainTemplatePath, dexMainTemplateLine,
                               dexContainers, newDexLine, 1);
    }
}