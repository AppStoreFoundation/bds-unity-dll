using System;

public class CustomBuildException : Exception
{
    public readonly string message;
    public CustomBuildException()
    {
    }

    public CustomBuildException(string mes) : base(mes)
    {
        message = mes;
    }

    public CustomBuildException(string mes, Exception inner) 
        : base(mes, inner)
    {
        message = mes;
    }
}

public class InvalidUnityVersion : CustomBuildException
{
    const string _message = "Unity version used is not compatible with" +
        "Appcoins Unity plugin. Please use a unity version greater than" +
        "'5.6'";

    public InvalidUnityVersion() : base(_message)
    {
    }

    public InvalidUnityVersion(string new_message)
        : base(new_message)
    {
    }

    public InvalidUnityVersion(Exception inner)
        : base(_message, inner)
    {
    }

    public InvalidUnityVersion(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class ExportProjectPathIsEqualToUnityProjectPathException : 
             CustomBuildException
{
    private const string _message = "Path chosed to store the exported " +
        "project is the same as the unity root project";

    public ExportProjectPathIsEqualToUnityProjectPathException() : base(_message)
    {
    }

    public ExportProjectPathIsEqualToUnityProjectPathException(string new_message)
        : base(new_message)
    {
    }

    public ExportProjectPathIsEqualToUnityProjectPathException(Exception inner)
        : base(_message, inner)
    {
    }

    public ExportProjectPathIsEqualToUnityProjectPathException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class ExportProjectPathIsNullException : CustomBuildException
{
    const string _message = "Invalid path chosed to save the exported project";

    public ExportProjectPathIsNullException() : base(_message)
    {
    }

    public ExportProjectPathIsNullException(string new_message)
        : base(new_message)
    {
    }

    public ExportProjectPathIsNullException(Exception inner)
        : base(_message, inner)
    {
    }

    public ExportProjectPathIsNullException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class ExportProjectFailedException : CustomBuildException
{
    const string _message = "Unity export failed. Check Unity log for more " +
        "detailed information.";

    public ExportProjectFailedException() : base(_message)
    {
    }

    public ExportProjectFailedException(string new_message)
        : base(new_message)
    {
    }

    public ExportProjectFailedException(Exception inner)
        : base(_message, inner)
    {
    }

    public ExportProjectFailedException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class TerminalProcessFailedException : CustomBuildException
{
    const string _message = "Terminal process failed. Check Unity log or the " +
        "{unity_proj_path}/Assets/AppcoinsUnity/Tools/ProcessError.out " +
        "file for more detailed information.";

    public TerminalProcessFailedException() : base(_message)
    {
    }

    public TerminalProcessFailedException(string new_message)
        : base(new_message)
    {
    }

    public TerminalProcessFailedException(Exception inner)
        : base(_message, inner)
    {
    }

    public TerminalProcessFailedException(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class ASFAppcoinsGameObjectNotFound : CustomBuildException
{
    const string _message = "ASFAppcoinsUnity prefab not found at any" +
        "open scene. You can find the prefab at {$unity_proj_path}/Assets" +
        "/AppcoinsUnity/Prefabs/";

    public ASFAppcoinsGameObjectNotFound() : base(_message)
    {
    }

    public ASFAppcoinsGameObjectNotFound(string new_message)
        : base(new_message)
    {
    }

    public ASFAppcoinsGameObjectNotFound(Exception inner)
        : base(_message, inner)
    {
    }

    public ASFAppcoinsGameObjectNotFound(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}

public class BDSAppcoinsGameObjectNotFound : CustomBuildException
{
    const string _message = "AppcoinsPurchasing prefab not found at any" +
        "open scene. You can find the prefab at {$unity_proj_path}/Assets" +
        "/AppcoinsUnity/Prefabs/";

    public BDSAppcoinsGameObjectNotFound() : base(_message)
    {
    }

    public BDSAppcoinsGameObjectNotFound(string new_message)
        : base(new_message)
    {
    }

    public BDSAppcoinsGameObjectNotFound(Exception inner)
        : base(_message, inner)
    {
    }

    public BDSAppcoinsGameObjectNotFound(string new_message,
        Exception inner) : base(new_message, inner)
    {
    }
}