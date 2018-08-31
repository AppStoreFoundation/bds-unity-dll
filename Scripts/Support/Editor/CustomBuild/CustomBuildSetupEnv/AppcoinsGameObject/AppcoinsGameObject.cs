using UnityEngine;

public abstract class AppcoinsGameObject
{
    protected readonly string mainTemplatePath = Application.dataPath +
                                                 "/Plugins/Android/" +
                                                 "mainTemplate.gradle";

    protected const string mainTemplateVarName = "APPCOINS_PREFAB";
    protected readonly string[] mainTemplateContainers = { "debug", "release" };
    protected const string toReplace = "{0}";
    protected const int numTimes = 2;

    internal abstract void CheckAppcoinsGameobject();
}
