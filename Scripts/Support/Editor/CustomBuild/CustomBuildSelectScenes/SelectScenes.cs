using UnityEditor;
using System.Collections;

// Get all the loaded scenes and asks the user what scenes he wants 
// to export by 'ExportScenesWindow' class.
public class SelectScenes
{
    public bool[] buildScenesEnabled;

    public string[] ScenesToString()
    {
        ArrayList pathScenes = new ArrayList();

        for(int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if(EditorBuildSettings.scenes[i].enabled)
            {
                pathScenes.Add(EditorBuildSettings.scenes[i].path);
            }
        }

        return (pathScenes.ToArray(typeof(string)) as string[]);
    }

    public SceneToExport[] GetAllOpenScenes()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
        SceneToExport[] scenes = new SceneToExport[sceneCount];

        for(int i = 0; i < sceneCount; i++)
        {
            UnityEngine.SceneManagement.Scene scene = 
                UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

            if(scenes[i] == null)
            {
                scenes[i] = new SceneToExport();
            }

            scenes[i].scene = scene;
            scenes[i].exportScene = scene.buildIndex >= 0 ? true : false;
        }

        return scenes;
    }

    public void AddAllOpenScenesToBuildSettings()
    {
        SceneToExport[] scenes = GetAllOpenScenes();

        EditorBuildSettingsScene[] buildScenes = 
            new EditorBuildSettingsScene[scenes.Length];

        for(int i = 0; i < scenes.Length; i++)
        {
            buildScenes[i] = new EditorBuildSettingsScene(scenes[i].scene.path, 
                                                          true);
        }

        EditorBuildSettings.scenes = buildScenes;
    }

    // Return a bool array. All indexes with true value correspond to an enabled
    // scene at BuildSettings scenes.
    public bool[] GetBuildSettingsScenesEnabled()
    {
        bool[] scenesEnabled = new bool[EditorBuildSettings.scenes.Length];
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        for(int i = 0; i < scenes.Length; i++)
        {
            scenesEnabled[i] = scenes[i].enabled;
        }

        return scenesEnabled;
    }

    public void UpdatedBuildScenes(bool[] enabledScenes)
    {
        EditorBuildSettingsScene[] newBuildScenes = 
            new EditorBuildSettingsScene[enabledScenes.Length];

        for(int i = 0; i < enabledScenes.Length; i++)
        {
            newBuildScenes[i] = 
                new EditorBuildSettingsScene(EditorBuildSettings.scenes[i].path,
                                             enabledScenes[i]);
        }

        EditorBuildSettings.scenes = newBuildScenes;
    }

}