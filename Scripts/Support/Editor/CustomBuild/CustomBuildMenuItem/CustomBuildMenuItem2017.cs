using UnityEditor;
using UnityEngine;

public static class CustomBuildMenuItem
{
    [MenuItem("AppCoins/Android Custom Build")]
    public static void AndroidCustomBuild()
    {
        CustomBuildSetupEnv customBuildSetup =
            new CustomBuildAndroidSetupEnv(new BDSAppcoinsGameObject());

        CustomBuildWindow customBuildWindow =
            ScriptableObject.CreateInstance<AndroidCustomBuildWindow>();

        CustomBuildUnityExport customBuildUnityExport =
            GetCustomBuildUnityExport2017();

        CustomBuildProjectBuild customBuildProjectBuild =
            new CustomBuildGradleProjectBuild();

        CustomBuildProjectInstall customBuildProjectInstall =
            new CustomBuildAdbProjectInstall();

        CustomBuildProjectRun customBuildProjectRun =
            new CustomBuildAdbProjectRun();

        CustomBuildErrorTitles eT = new CustomBuildAndroidErrorTitles();

        CustomBuild c = new CustomBuild(customBuildSetup, customBuildWindow,
                                        customBuildUnityExport,
                                        customBuildProjectBuild,
                                        customBuildProjectInstall,
                                        customBuildProjectRun,
                                        eT
                                       );
        c.RunProcess();

    }


    //private static CustomBuildUnityExport GetCustomBuildUnityExport5_6()
    //{
    //    BuildTarget bT = BuildTarget.Android;
    //    BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
    //    ICustomBuildTarget target = new CustomBuildTargetAndroid();

    //    return new CustomBuildUnityExport5_6(bT, bO, target);
    //}

    private static CustomBuildUnityExport GetCustomBuildUnityExport2017()
    {
        BuildTarget bT = BuildTarget.Android;
        BuildTargetGroup bG = BuildTargetGroup.Android;
        BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
        ICustomBuildTarget target = new CustomBuildTargetAndroid();

        return new CustomBuildUnityExport2017OrLower(bT, bG, bO, target);
    }

    //private static CustomBuildUnityExport GetCustomBuildUnityExport2018()
    //{
    //    BuildTarget bT = BuildTarget.Android;
    //    BuildTargetGroup bG = BuildTargetGroup.Android;
    //    BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
    //    ICustomBuildTarget target = new CustomBuildTargetAndroid();

    //    return new CustomBuildUnityExport2018(bT, bO, bG, target);
    //}


    [MenuItem("AppCoins/Parameters")]

    public static void ParameterWindow()
    {
        ParameterWindow parameter = new ParameterWindow();
        EditorWindow.GetWindow(typeof(ParameterWindow));
    }
}