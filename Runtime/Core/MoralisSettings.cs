
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

namespace Moralis.Web3UnitySdk
{
    public class MoralisSettings
    {
        private static string MoralisDataFilename = "MoralisDataScriptableObject";
        public static MoralisDataScriptableObject MoralisData;

                /// <summary>
        /// Loads the Moralis Data Setting sasset. If it does not exist, create it.
        /// </summary>
        /// <param name="reload"></param>
        public static void LoadOrCreateSettings(bool reload = false)
        {
            if (reload)
            {
                // Force reload of the MoralisData Settings.
                MoralisData = null;    
            }
            else if (MoralisData != null)
            {
                // Moralis Data setting have already been loaded.
                return;
            }

            // Try to load the resource / asset (MoralisDataScriptableObject
            // a.k.a. Moralis Data Settings)
            MoralisData = (MoralisDataScriptableObject)Resources.Load(MoralisDataFilename, typeof(MoralisDataScriptableObject));
            
            // If Moralis Data Setting were loaded successfully, all is well,
            // exit the method.
            if (MoralisData != null)
            {
                return;
            }

#if UNITY_EDITOR
            // The MoralisDataScriptableObject a.k.a Moralis Data Settings does not exist so create it.
            if (MoralisData == null)
            {
                // Create a fresh instance of the Moralis Datat Setting sasset.
                MoralisData = (MoralisDataScriptableObject)MoralisDataScriptableObject.CreateInstance("MoralisDataScriptableObject");
                
                if (MoralisData == null)
                {
                    Debug.LogError("Failed to create MoralisDataScriptableObject. Moralis is unable to run this way. If you deleted it from the project, reload the Editor.");
                    return;
                }
            }

            string punResourcesDirectory = Editor.UnityFileHelper.FindMoralisAssetFolder() + "Resources/";
            string serverSettingsAssetPath = punResourcesDirectory + MoralisDataFilename + ".asset";
            string serverSettingsDirectory = Path.GetDirectoryName(serverSettingsAssetPath);

            if (!Directory.Exists(serverSettingsDirectory))
            {
                Directory.CreateDirectory(serverSettingsDirectory);
                AssetDatabase.ImportAsset(serverSettingsDirectory);
            }

            if (!File.Exists(serverSettingsAssetPath))
            {
                AssetDatabase.CreateAsset(MoralisData, serverSettingsAssetPath);
            }

            AssetDatabase.SaveAssets();

            // if the project does not have PhotonServerSettings yet, enable "Development Build" to use the Dev Region.
            EditorUserBuildSettings.development = true;
#endif
        }
    }
}