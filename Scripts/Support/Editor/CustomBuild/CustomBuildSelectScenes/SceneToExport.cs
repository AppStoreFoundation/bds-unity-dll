// Custom class to save the loaded scenes and a bool for each scene 
// that tells us if the user wants to export such scene or not.

public class SceneToExport
{
    private bool _exportScene;
    public bool exportScene
    {
        get { return _exportScene; }
        set { _exportScene = value; }
    }

    private UnityEngine.SceneManagement.Scene _scene;
    public UnityEngine.SceneManagement.Scene scene
    {
        get { return _scene; }
        set { _scene = value; }
    }
}