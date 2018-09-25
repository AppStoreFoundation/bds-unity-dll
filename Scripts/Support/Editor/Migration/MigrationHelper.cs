using UnityEngine;

using System;
using System.IO;

public static class MigrationHelper {
    public static void DeleteOlderFiles() {

        string scriptsPath = Application.dataPath + "/AppcoinsUnity";
        string editorScriptsPath = scriptsPath + "/Editor";

        //string[] olderScriptFiles = { "AppcoinsPurchaser", "AppcoinsSku",
            //"AppcoinsUnity", "BashUtils", 
            //"AppCoinsUnityPluginTests2018", 
            //"AppCoinsUnityPluginTests2017", 
            //"AppCoinsUnityPluginTests5_6",
            //"AppCoinsUnityPluginEditorMode2018", 
            //"AppCoinsUnityPluginEditorMode5_6",
            //"AppCoinsUnityPluginEditorMode2017" };

        string[] olderEditorScriptFiles = { "appcoins-unity-support-2018",
            "appcoins-unity-support-5_6", "appcoins-unity-support-2017",
            "AppCoinsUnitySupport2018", "AppCoinsUnitySupport2017",
            "AppCoinsUnitySupport5_6" };

        //DeleteFiles(scriptsPath, olderScriptFiles);
        DeleteFiles(editorScriptsPath, olderEditorScriptFiles);
    }

    private static void DeleteFiles(string dirPath, string[] filesToDelete) {
        foreach(string filePath in Directory.GetFiles(dirPath))
        {
            string fName = Path.GetFileName(filePath);

            if (Array.Exists(filesToDelete, dF => dF.Equals(fName)))
            {
                File.Delete(filePath);
            }
        }
    }
}