using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

// Draw the window for the user select what scenes he wants to export
// and configure player settings.

public abstract class CustomBuildWindow : EditorWindow
{
    protected static CustomBuildWindow instance;
    protected CustomBuildWindow innerInstance;
    public Vector2 scrollViewVector = Vector2.zero;

    protected internal BuildStage stage;
    protected internal SelectScenes selector;
    protected internal bool[] buildScenesEnabled = null;
    protected internal UnityEvent unityEvent;

    //Create the custom Editor Window
    public static void CreateCustomBuildWindow(BuildStage s,
                                               CustomBuildWindow w, 
                                               SelectScenes sel,
                                               UnityEvent ev
                                              )
    {
        CustomBuildWindow.instance = (CustomBuildWindow)
            EditorWindow.GetWindowWithRect(
                typeof(CustomBuildWindow),
                new Rect(0, 0, 600, 500),
                true,
                "Custom Build Settings"
            );

        instance.stage = s;
        instance.innerInstance = w;
        instance.unityEvent = ev;
        instance.selector = sel;
        instance.buildScenesEnabled = 
            instance.selector.GetBuildSettingsScenesEnabled();

        instance.minSize = new Vector2(600, 500);
        instance.autoRepaintOnSceneChange = true;

        instance.innerInstance.LoadCustomBuildPrefs();
        instance.Show();
    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    void OnGUI()
    {
        switch (instance.stage)
        {
            case BuildStage.IDLE:
                instance.innerInstance.IdleGUI();
                break;
            case BuildStage.UNITY_EXPORT:
                instance.innerInstance.UnityExportGUI();
                break;
            case BuildStage.PROJECT_BUILD:
                instance.innerInstance.ProjectBuildGUI();
                break;
            case BuildStage.PROJECT_INSTALL:
                instance.innerInstance.ProjectInstallGUI();
                break;
            case BuildStage.PROJECT_RUN:
                instance.innerInstance.ProjectRunGUI();
                break;
            case BuildStage.DONE:
                Close();
                break;
        }
    }

    protected abstract void IdleGUI();

    protected abstract void UnityExportGUI();

    protected abstract void ProjectBuildGUI();

    protected abstract void ProjectInstallGUI();

    protected abstract void ProjectRunGUI();

    protected abstract void LoadCustomBuildPrefs();

    protected abstract void SetCustomBuildPrefs();

    protected string HandleCopyPaste(int controlID)
    {
        if (controlID == GUIUtility.keyboardControl)
        {
            if (Event.current.type == EventType.KeyUp && 
                (Event.current.modifiers == EventModifiers.Control ||
                 Event.current.modifiers == EventModifiers.Command
                )
               )
            {
                if (Event.current.keyCode == KeyCode.C)
                {
                    Event.current.Use();
                    TextEditor editor = (TextEditor)
                        GUIUtility.GetStateObject(typeof(TextEditor),
                                                  GUIUtility.keyboardControl
                                                 );
                    editor.Copy();
                }

                else if (Event.current.keyCode == KeyCode.V)
                {
                    Event.current.Use();
                    TextEditor editor = (TextEditor)
                        GUIUtility.GetStateObject(typeof(TextEditor), 
                                                  GUIUtility.keyboardControl
                                                 );
                    editor.Paste();
                    return editor.text;
                }
            }
        }
        
        return null;
    }
}