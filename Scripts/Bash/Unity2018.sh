mkdir -p ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts
mkdir -p ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts/Editor
cp ./unity-2018/bin/Debug/bds-unity-2018.dll ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts
cp ./unity-editor-2018/bin/Debug/bds-unity-editor-2018.dll ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts/Editor
cp ./unity-editor-2018/bin/Debug/appcoins-unity-support-2018.dll ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts/Editor
cp ./Scripts/ToCopy/* ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts
cp ./Scripts/ToCopy/Editor/* ../bds-unity-plugin/AppcoinsUnityPlugin/Assets/AppcoinsUnity/Scripts/Editor