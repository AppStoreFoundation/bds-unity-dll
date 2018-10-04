using UnityEngine;
using Appcoins.Purchasing;

public class BDSAppcoinsGameObject : AppcoinsGameObject
{
    private AppcoinsPurchasing bdsGameObject;
    private const string appcoinsNameNewLine = "resValue \"string\", " +
        "\"APPCOINS_PREFAB\", \"{0}\"";

    private void FindAppcoinsGameObject()
    {
        var foundObjects = Resources.FindObjectsOfTypeAll<AppcoinsPurchasing>();

        if (foundObjects.Length == 0)
        {
            throw new BDSAppcoinsGameObjectNotFound();
        }

        bdsGameObject = foundObjects[0];
    }

    public override void CheckAppcoinsGameobject()
    {
        FindAppcoinsGameObject();

        string newLine = 
            appcoinsNameNewLine.Replace(toReplace, 
                                        bdsGameObject.gameObject.name);

        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateVarName,
                               mainTemplateContainers, newLine);
    }
}
