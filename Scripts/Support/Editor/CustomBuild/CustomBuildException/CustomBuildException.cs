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