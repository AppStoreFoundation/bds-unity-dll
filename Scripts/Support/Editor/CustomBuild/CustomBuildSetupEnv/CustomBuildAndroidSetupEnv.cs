using UnityEditor;

public class CustomBuildAndroidSetupEnv : CustomBuildSetupEnv
{
    private const string defaultUnityPackage = "com.Company.ProductName";

    public CustomBuildAndroidSetupEnv(AppcoinsGameObject a) : base(a) {}

    internal override void Setup()
    {
        base.Setup();

        // Check if the active platform is Android. If it isn't change it
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Android, BuildTarget.Android);
        }

        // Check if min sdk version is lower than 21. If it is, set it to 21
        if (PlayerSettings.Android.minSdkVersion < 
            AndroidSdkVersions.AndroidApiLevel21
           )
        {
            PlayerSettings.Android.minSdkVersion = 
                AndroidSdkVersions.AndroidApiLevel21;
        }

        //// Check if the bunde id is the default one and change it if it to 
        //// avoid that error
        //if (PlayerSettings.applicationIdentifier.Equals(defaultUnityPackage))
        //{
        //    PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, 
        //                                            "com.aptoide.appcoins");
        //}

        // Export Project with gradle format (template)
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        //Make sure all non relevant errors go away
        UnityEngine.Debug.ClearDeveloperConsole();
        UnityEngine.Debug.Log("Successfully integrated Appcoins Unity plugin!");
    }
}