using UnityEngine;
using UnityEditor;
using Appcoins.Purchasing;

public class BDSAppcoinsGameObject : AppcoinsGameObject
{
    private AppcoinsPurchasing bdsGameObject;
    protected const string mainTemplateWalletAddressVarName = "APPCOINS_WALLET_ADDRESS";
    protected const string mainTemplateDeveloperKeyVarName = "APPCOINS_DEVELOPER_KEY";
    protected const string mainTemplatePOAVarName = "APPCOINS_ENABLE_POA";
    protected const string mainTemplateDEBUGVarName = "APPCOINS_ENABLE_DEBUG";

    private const string appcoinsWalletAddressNewLine = "resValue \"string\", " +
        "\"APPCOINS_WALLET_ADDRESS\", \"{0}\"";
    private const string appcoinsDeveloperKeyNewLine = "resValue \"string\", " +
        "\"APPCOINS_DEVELOPER_KEY\", \"{0}\"";
    private const string appcoinsNameNewLine = "resValue \"string\", " +
        "\"APPCOINS_PREFAB\", \"{0}\"";
    private const string poaEnabledNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_POA\", \"{0}\"";
    private const string debugEnabledNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_DEBUG\", \"{0}\"";

    public override void CheckAppcoinsGameobject()
    {

        string newLine = "";
        //appcoinsNameNewLine.Replace(toReplace, 
        //bdsGameObject.gameObject.name);

        //Tools.ChangeLineInFile(mainTemplatePath, mainTemplateVarName,
        //mainTemplateContainers, newLine);

        string walletAddress = EditorPrefs.GetString(AppcoinsConstants.APPCOINS_WALLET_ADDRESS_KEY, "");
        Debug.Log("Wallet address is " + walletAddress);
        newLine = appcoinsWalletAddressNewLine.Replace(toReplace, walletAddress);
        Debug.Log("Wallet address newline is " + newLine);
        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateWalletAddressVarName,
                               mainTemplateContainers, newLine);
        
        string developerKey = EditorPrefs.GetString(AppcoinsConstants.APPCOINS_PUBLIC_KEY_KEY, "");
        Debug.Log("Developer key is " + developerKey);
        newLine = appcoinsDeveloperKeyNewLine.Replace(toReplace, developerKey);
        Debug.Log("Developer key newline is " + newLine);
        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateDeveloperKeyVarName,
                               mainTemplateContainers, newLine);

        //Handle POA flag
        bool usesUserAcquisitionSDK = EditorPrefs.GetBool(AppcoinsConstants.APPCOINS_USE_UA_KEY, false);
        Debug.Log("use ads is " + usesUserAcquisitionSDK);
        newLine = poaEnabledNewLine.Replace(toReplace, usesUserAcquisitionSDK.ToString().ToLower());

        Debug.Log("use ads newline is " + newLine);
        Tools.ChangeLineInFile(mainTemplatePath, mainTemplatePOAVarName,
                               mainTemplateContainers, newLine);

        //Handle Debug network flag
        bool shouldLog = EditorPrefs.GetBool(AppcoinsConstants.APPCOINS_SHOULD_LOG_KEY, false);
        newLine = debugEnabledNewLine.Replace(toReplace, shouldLog.ToString().ToLower());
        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateDEBUGVarName,
                               mainTemplateContainers, newLine);
    }
}
