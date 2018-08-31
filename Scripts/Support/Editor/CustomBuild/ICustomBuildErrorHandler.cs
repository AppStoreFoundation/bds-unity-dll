using System;

public interface ICustomBuildErrorHandler
{
    void HandleError(Exception e);
}