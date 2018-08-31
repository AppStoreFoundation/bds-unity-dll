using UnityEditor;

using System;

public class CustomBuildEditorErrorHandler : ICustomBuildErrorHandler
{
    void ICustomBuildErrorHandler.HandleError(Exception e)
    {
        EditorUtility.DisplayDialog("Custom Build Error:",
                                    e.Message,
                                    "Got it"
                                   );
    }
}