public abstract class CustomBuildSetupEnv
{
    AppcoinsGameObject appcoinsGameObject;

    public CustomBuildSetupEnv(AppcoinsGameObject a)
    {
        appcoinsGameObject = a;
    }

    internal virtual void Setup()
    {
        appcoinsGameObject.CheckAppcoinsGameobject();
    }
}