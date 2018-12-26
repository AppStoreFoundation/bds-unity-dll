using UnityEngine;
using Appcoins.Purchasing;

public class BDSAppcoinsGameObject : AppcoinsGameObject
{
    private AppcoinsPurchasing bdsGameObject;
    protected const string mainTemplatePOAVarName = "APPCOINS_ENABLE_POA";
    protected const string mainTemplateDEBUGVarName = "APPCOINS_ENABLE_DEBUG";

    private const string appcoinsNameNewLine = "resValue \"string\", " +
        "\"APPCOINS_PREFAB\", \"{0}\"";
    private const string poaEnabledNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_POA\", \"{0}\"";
    private const string debugEnabledNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_DEBUG\", \"{0}\"";

    private void FindAppcoinsGameObject()
    {
        var foundObject = (AppcoinsPurchasing)FindObjectOfType(typeof(AppcoinsPurchasing));

        if (foundObject == null)
        {
            throw new BDSAppcoinsGameObjectNotFound();
        }

        bdsGameObject = foundObject;
    }

    public override void CheckAppcoinsGameobject()
    {
        FindAppcoinsGameObject();

        string newLine = 
            appcoinsNameNewLine.Replace(toReplace, 
                                        bdsGameObject.gameObject.name);

        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateVarName,
                               mainTemplateContainers, newLine);

        //Handle POA flag
        Debug.Log("use ads is " + bdsGameObject.UsesAdsSDK());
        newLine = poaEnabledNewLine.Replace(toReplace, bdsGameObject.UsesAdsSDK().ToString().ToLower());

        Debug.Log("use ads newline is " + newLine);
        Tools.ChangeLineInFile(mainTemplatePath, mainTemplatePOAVarName,
                               mainTemplateContainers, newLine);

        //Handle Debug network flag
        newLine = debugEnabledNewLine.Replace(toReplace, (bdsGameObject.UsesLog()).ToString().ToLower());
        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateDEBUGVarName,
                               mainTemplateContainers, newLine);
    }
}
