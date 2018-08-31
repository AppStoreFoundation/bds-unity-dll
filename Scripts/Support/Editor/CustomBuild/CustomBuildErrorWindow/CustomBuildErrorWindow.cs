using UnityEditor;
using UnityEngine;

using System;

public class CustomBuildErrorWindow : EditorWindow
{
    protected static CustomBuildErrorWindow instance;

    private string[] errorTitles;
    private BuildStage failStage;
    private string errorMessage;

    public Vector2 scrollViewVector = Vector2.zero;

    //Create the custom Editor Window
    public static void CreateCustomBuildErrorWindow()
    {
        instance = (CustomBuildErrorWindow)
            EditorWindow.GetWindowWithRect(
                typeof(CustomBuildErrorWindow),
                new Rect(0, 0, 600, 500),
                false,
                "Custom Build Errors"
            );

        instance.minSize = new Vector2(600, 500);
        instance.Show();
    }

    void OnEnable()
    {
        instance = this;
    }

    void OnDisable()
    {
        instance = null;
    }

    //public void OnInspectorUpdate()
    //{
    //    // This will only get called 10 times per second.
    //    Repaint();
    //}

    void OnGUI()
    {
        EditorWindow.FocusWindowIfItsOpen(typeof(CustomBuildErrorWindow));
        BuildStage[] allStages = {BuildStage.UNITY_EXPORT,
            BuildStage.PROJECT_BUILD, BuildStage.PROJECT_INSTALL,
            BuildStage.PROJECT_RUN
        };

        LoadErrorEditorPrefbs(allStages);

        if (instance != null)
        {
            instance.ErrorGUI(allStages);
        }

        else
        {
            instance = (CustomBuildErrorWindow)
            EditorWindow.GetWindowWithRect(
                typeof(CustomBuildErrorWindow),
                new Rect(0, 0, 600, 500),
                false,
                "Custom Build Errors"
            );
        }
    }

    protected virtual void ErrorGUI(BuildStage[] allStages)
    {
        float constMul = 6.8F;
        Texture2D success;
        Texture2D fail;

        int height = 10;

        int failStageIndex = ArrayUtility.IndexOf<BuildStage>(allStages,
                                                              failStage);

        int i = 0;
        while (i < allStages.Length)
        {
            GUI.Label(new Rect(5, height, 590, 20), errorTitles[i]);

            if (i < failStageIndex)
            {
                success = (Texture2D)Resources.Load(
                    "icons/success", 
                    typeof(Texture2D)
                );

                GUI.DrawTexture(
                    new Rect(
                        errorTitles[i].Length * constMul, 
                        height, 
                        20, 
                        20
                    ), 
                    success
                );
            }

            else
            {
                fail = (Texture2D)Resources.Load(
                    "icons/false", 
                    typeof(Texture2D)
                );

                GUI.DrawTexture(
                    new Rect(
                        errorTitles[i].Length * constMul,
                        height,
                        20,
                        20
                    ),
                    fail
                );
            }

            height += 40;
            i++;
        }

        GUI.Label(new Rect(10, height, 580, height + 100), "Error:\n" + 
                  errorMessage, GUI.skin.textArea);

        if (GUI.Button(new Rect(530, 470, 60, 20), "Got it"))
        {
            instance.Close();
        }
    }

    protected void LoadErrorEditorPrefbs(BuildStage[] allStages)
    {
        string[] genericErrorTitles = {
            "Export Unity Project: ",
            "Build Exported Project: ",
            "Install .apk to device: ",
            "Run .apk in the device: "
        };

        failStage = (BuildStage) EditorPrefs.GetInt("appcoins_error_stage", 0);
        errorMessage = EditorPrefs.GetString("appcoins_error_message", "");

        errorTitles = new string[allStages.Length];
        for (int i = 0; i < allStages.Length; i++)
        {
            errorTitles[i] = 
                EditorPrefs.GetString("appcoins_error_title_" + i.ToString(), 
                                      genericErrorTitles[i]);
        }
    }
}