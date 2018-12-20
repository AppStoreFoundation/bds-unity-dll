using UnityEditor;
using UnityEngine;

using System;

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

    public static void SetupBuild(){
    
        CustomBuildSetupEnv d =
            new CustomBuildAndroidSetupEnv(new BDSAppcoinsGameObject());

        //Start setup and inform the developer that it has successfully been done
        d.Setup();
    }

    private static CustomBuildUnityExport GetCustomBuildUnityExport2018()
    {
        BuildTarget bT = BuildTarget.Android;
        BuildTargetGroup bG = BuildTargetGroup.Android;
        BuildOptions bO = BuildOptions.AcceptExternalModificationsToPlayer;
        ICustomBuildTarget target = new CustomBuildTargetAndroid();

        return new CustomBuildUnityExport2018(bT, bO, bG, target);
    }
}