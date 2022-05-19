using System.Collections.Generic;
using MoralisUnity.Sdk.Constants;
using UnityEditor;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{

	/// <summary>
	/// Unity offers https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ScriptableSingleton_1.html
	/// but it was throwing "ScriptableSingleton already exists. Did you query the singleton in a constructor?"
	/// So here is a custom implementation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class CustomSingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		private static T _instance = null;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					// Instance must be manually created by developer via [MenuItem] and put any /Resources/ folder.
					T[] results = Resources.FindObjectsOfTypeAll<T>();

					if (results.Length == 0)
					{
						//Fix the code if this happens
						Debug.LogError("CustomSingletonScriptableObject: Developer must use [MenuItem] to first create type of " + typeof(T).ToString());
						return null;
					}

					if (results.Length > 1)
					{
						//Fix the code if this happens
						Debug.LogError("CustomSingletonScriptableObject: Results length is greater than 1 of " + typeof(T).ToString());
						return null;
					}

					_instance = results[0]; 
					_instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
				}
				return _instance;
			}
		}
	}

	[CreateAssetMenu (fileName = Title, menuName = MoralisConstants.PathMoralisCreateAssetMenu + "/" + ExampleConstants.MoralisAPIExamples +"/" + Title)]
    public class ExampleDiskStorage: CustomSingletonScriptableObject<ExampleDiskStorage>
	{
        //  Properties ------------------------------------
        public List<SceneAsset> SceneAssets { get { return _sceneAssets; } }

        //  Fields ----------------------------------------
        private const string Title = "ExampleLocalStorage";

        [SerializeField]
        private List<SceneAsset> _sceneAssets = null;

        //  Methods ---------------------------------------
    }
}
