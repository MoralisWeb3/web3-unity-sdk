using System.Collections.Generic;
using MoralisUnity.Sdk.Constants;
using UnityEditor;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
    //[CreateAssetMenu (fileName = Title, menuName = MoralisConstants.PathMoralisCreateAssetMenu + "/" + Title)]
    public class ExampleLocalStorage : ScriptableSingleton<ExampleLocalStorage>
    {
        //  Properties ------------------------------------
        public List<SceneAsset> SceneAssets { get { return _sceneAssets; }}
        
        //  Fields ----------------------------------------
        private const string Title = "ExampleLocalStorage";

        [SerializeField]
        private List<SceneAsset> _sceneAssets = null;
        
        //  Methods ---------------------------------------
    }
}
