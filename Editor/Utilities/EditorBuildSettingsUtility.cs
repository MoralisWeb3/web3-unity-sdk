using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MoralisUnity.Sdk.Utilities
{
    public static class EditorBuildSettingsUtility 
    {
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        
        //  Methods ---------------------------------------
        public static void AddScenesToBuildSettings(List<SceneAsset> sceneAssets)
        {
            // Find valid Scene paths and make a list of EditorBuildSettingsScene
            List<EditorBuildSettingsScene> existingScenes = EditorBuildSettings.scenes.ToList();
            
            foreach (SceneAsset sceneAsset in sceneAssets)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                
                // Remove if exists (to improve sort)
                bool alreadyExists = existingScenes.Any(item => item.path == scenePath);
                if (alreadyExists)
                {
                    existingScenes.RemoveAll(item => item.path == scenePath);
                }
                
                // Add 
                if (!string.IsNullOrEmpty(scenePath))
                {
                    existingScenes.Add(new EditorBuildSettingsScene(scenePath, true)); 
                }
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = existingScenes.ToArray();
        }
    }
}
