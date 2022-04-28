using UnityEngine;
using UnityEditor;
using System.IO;

namespace MoralisUnity
{
    public class MoralisSettings
    {
 		private static MoralisServerSettings moralisData;
        
        public static MoralisServerSettings MoralisData
        {
            get
            {
                if (moralisData == null)
                {
                    LoadOrCreateSettings();
                }

                return moralisData;
            }
            private set { moralisData = value; }
        }

        private static string MoralisDataFilename = "MoralisServerSettings";

        /// <summary>
        /// Loads the Moralis Data Setting sasset. If it does not exist, create it.
        /// </summary>
        /// <param name="reload"></param>
        public static void LoadOrCreateSettings(bool reload = false)
        {
            if (reload)
            {
                // Force reload of the moralisData Settings.
                moralisData = null;    
            }
            else if (moralisData != null)
            {
                // Moralis Data setting have already been loaded.
                return;
            }

            // Try to load the resource / asset (MoralisServerSettings
            // a.k.a. Moralis Data Settings)
            moralisData = (MoralisServerSettings)Resources.Load(MoralisDataFilename, typeof(MoralisServerSettings));
            
            // If Moralis Data Setting were loaded successfully, all is well,
            // exit the method.
            if (moralisData != null)
            {
                return;
            }

#if UNITY_EDITOR
            // The MoralisServerSettings a.k.a Moralis Data Settings does not exist so create it.
            if (moralisData == null)
            {
                // Create a fresh instance of the Moralis Datat Setting sasset.
                moralisData = (MoralisServerSettings)MoralisServerSettings.CreateInstance("MoralisServerSettings");
                
                if (moralisData == null)
                {
                    Debug.LogError("Failed to create MoralisServerSettings. Moralis is unable to run this way. If you deleted it from the project, reload the Editor.");
                    return;
                }
            }

            string moralisResourcesDirectory = UnityFileHelper.FindMoralisAssetFolder() ;
            string serverSettingsAssetPath = moralisResourcesDirectory + MoralisDataFilename + ".asset";
            string serverSettingsDirectory = Path.GetDirectoryName(serverSettingsAssetPath);

            if (!Directory.Exists(serverSettingsDirectory))
            {
                Directory.CreateDirectory(serverSettingsDirectory);
                AssetDatabase.ImportAsset(serverSettingsDirectory);
            }

            if (!File.Exists(serverSettingsAssetPath))
            {
                AssetDatabase.CreateAsset(moralisData, serverSettingsAssetPath);
            }

            AssetDatabase.SaveAssets();
#endif
        }
    }
}