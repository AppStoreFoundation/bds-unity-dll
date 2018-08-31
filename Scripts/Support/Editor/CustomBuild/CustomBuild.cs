using UnityEditor;
using UnityEngine.Events;

using System;

public enum BuildStage
{
    SETUP_ENV,
    IDLE,
    UNITY_EXPORT,
    PROJECT_BUILD,
    PROJECT_INSTALL,
    PROJECT_RUN,
    DONE,
}

public class CustomBuild
{
    private SelectScenes scenesSelector;
    private string[] scenesPath = null;
    private string projPath;

    private CustomBuildSetupEnv customBuildSetup;
    private CustomBuildWindow customBuildWindow;
    private CustomBuildUnityExport customBuildUnityExport;
    private CustomBuildProjectBuild customBuildProjectBuild;
    private CustomBuildProjectInstall customBuildProjectInstall;
    private CustomBuildProjectRun customBuildProjectRun;

    private CustomBuildErrorTitles errorTitles;

    private UnityEvent idleClosed;

    public BuildStage stage;

    public CustomBuild(CustomBuildSetupEnv setupEnv, CustomBuildWindow window,
                       CustomBuildUnityExport unityExport, 
                       CustomBuildProjectBuild projectBuild,
                       CustomBuildProjectInstall projectInstall,
                       CustomBuildProjectRun projectRun,
                       CustomBuildErrorTitles eT
                      )
    {
        scenesSelector = new SelectScenes();

        customBuildSetup = setupEnv;
        customBuildWindow = window;
        customBuildUnityExport = unityExport;
        customBuildProjectBuild = projectBuild;
        customBuildProjectInstall = projectInstall;
        customBuildProjectRun = projectRun;

        errorTitles = eT;

        idleClosed = new UnityEvent();
    }

    // Run all custom build phases
    public virtual void RunProcess()
    {
        // Phase 1: Setup Enviornment
        StateSetupEnv();
        customBuildSetup.Setup();

        // Phase 2: GUI (Chose custom build process)
        StateBuildIdle();
        CustomBuildWindow.CreateCustomBuildWindow(stage,
                                                  customBuildWindow, 
                                                  scenesSelector,
                                                  idleClosed
                                                 );
        idleClosed.AddListener(
            delegate
            {
                scenesPath = scenesSelector.ScenesToString();
                RunInstalationProcess();
            }
        );
    }

    public virtual void RunInstalationProcess()
    {
        errorTitles.SetErrorTitles();

        try
        {
            // Phase 3: Export Unity Project
            StateUnityExport();
            customBuildUnityExport.UnityExport(stage, scenesPath, out projPath);
        }
        catch (ExportProjectPathIsEqualToUnityProjectPathException e)
        {
            HandleExceptions(e);
            return;
        }
        catch (ExportProjectPathIsNullException e)
        {
            HandleExceptions(e);
            return;
        }
        catch (ExportProjectFailedException e)
        {
            HandleExceptions(e);
            return;
        }

        try
        {
            // Phase 5: Build Exported Project
            StateProjectBuild();
            customBuildProjectBuild.BuildProject(stage, projPath);
        }
        catch (TerminalProcessFailedException e)
        {
            HandleExceptions(e);
            return;
        }

        try
        {
            // Phase 6: Intall apk
            StateProjectInstall();
            customBuildProjectInstall.InstallProject(stage, projPath);
        }
        catch (TerminalProcessFailedException e)
        {
            HandleExceptions(e);
            return;
        }

        try
        {
            // Phase 7: Run apk
            StateProjectRun();
            customBuildProjectRun.RunProject(stage, projPath);
        }
        catch (TerminalProcessFailedException e)
        {
            HandleExceptions(e);
            return;
        }

        EditorUtility.DisplayDialog("Custom Build", "Custom Build Completed " +
                                    "whitout any errors", "OK");
    }

    private void HandleExceptions(Exception e)
    {
        EditorPrefs.SetInt("appcoins_error_stage", (int)stage);
        EditorPrefs.SetString("appcoins_error_message", e.Message);
        CustomBuildErrorWindow.CreateCustomBuildErrorWindow();
    }

    #region State Handling

    private void ChangeStage(BuildStage theStage)
    {
        stage = theStage;
    }

    public void StateSetupEnv()
    {
        ChangeStage(BuildStage.SETUP_ENV);
    }

    public void StateBuildIdle()
    {
        ChangeStage(BuildStage.IDLE);
    }

    public void StateUnityExport()
    {
        ChangeStage(BuildStage.UNITY_EXPORT);
    }

    public void StateProjectBuild()
    {
        ChangeStage(BuildStage.PROJECT_BUILD);
    }

    public void StateProjectInstall()
    {
        ChangeStage(BuildStage.PROJECT_INSTALL);
    }

    public void StateProjectRun()
    {
        ChangeStage(BuildStage.PROJECT_RUN);
    }

    #endregion
}
