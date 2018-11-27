using UnityEditor;
using UnityEngine;

using System;

public static class CustomBuildMenuItem
{
    [MenuItem("AppCoins/Android Custom")]

    public static void AndroidCustomBuild()
    {
        CustomBuildSetupEnv customBuildSetup = 
            new CustomBuildAndroidSetupEnv(new BDSAppcoinsGameObject());

        CustomBuildWindow customBuildWindow = 
            ScriptableObject.CreateInstance<AndroidCustomBuildWindow>();

        CustomBuildUnityExport customBuildUnityExport =
            GetCustomBuildUnityExport2018();

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

    [MenuItem("AppCoins/Setup")]

    public static void SetupBuild(){

        CustomBuildSetupEnv customBuildSetup =
            new CustomBuildAndroidSetupEnv(new BDSAppcoinsGameObject());

        EditorUtility.DisplayDialog("Setup", "Setup runned successfully!", "Ok");

    }

    //private static CustomBuildUnityExport GetCustomBuildUnityExport5_6()
    //{
    //    BuildTarget bT = BuildTarget.Android;
    //    BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
    //    ICustomBuildTarget target = new CustomBuildTargetAndroid();

    //    return new CustomBuildUnityExport5_6(bT, bO, target);
    //}

    //private static CustomBuildUnityExport GetCustomBuildUnityExport2017()
    //{
    //    BuildTarget bT = BuildTarget.Android;
    //    BuildTargetGroup bG = BuildTargetGroup.Android;
    //    BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
    //    ICustomBuildTarget target = new CustomBuildTargetAndroid();

    //    return new CustomBuildUnityExport2017(bT, bO, bG, target);
    //}

    private static CustomBuildUnityExport GetCustomBuildUnityExport2018()
    {
        BuildTarget bT = BuildTarget.Android;
        BuildTargetGroup bG = BuildTargetGroup.Android;
        BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
        ICustomBuildTarget target = new CustomBuildTargetAndroid();

        return new CustomBuildUnityExport2018(bT, bO, bG, target);
    }
}