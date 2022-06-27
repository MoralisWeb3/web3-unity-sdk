using UnityEngine;
using UnityEditor;
using MoralisUnity.Sdk.Constants;
using MoralisUnity.Sdk.UI.ReadMe;

namespace MoralisUnity.Sdk.MenuItems
{
	/// <summary>
	/// The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.
	/// <see cref="https://docs.unity3d.com/ScriptReference/MenuItem.html"/>
	/// </summary>
	public static class MoralisMenuItems
	{
		[MenuItem( MoralisConstants.PathMoralisWindowMenu + "/" + MoralisConstants.Open + " " + "ReadMe", false, 0 )]
		static void OpenReadMe()
		{
			ReadMeEditor.SelectReadmeGuid("f28fe356e1effe947938cac2b8ba360a");
		}
		
		
		[MenuItem( MoralisConstants.PathMoralisWindowMenu + "/" + "Select AuthenticationKit", false, 30 )]
		static void SelectAuthenticationKit()
		{
			string path = AssetDatabase.GUIDToAssetPath("a41feed31bcc36541a7a9505212ddc63");
			if (string.IsNullOrEmpty(path)) return;
			var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
			if (obj == null) return;
            
			Selection.activeObject = obj;
			EditorGUIUtility.PingObject(obj);
		}
		
		
		[MenuItem( "GameObject/" + MoralisConstants.PathMoralisCreateAssetMenu + "/" + "AuthenticationKit", false, 30 )]
		static void CreateAuthenticationKit()
		{
			string path = AssetDatabase.GUIDToAssetPath("a41feed31bcc36541a7a9505212ddc63");
			if (string.IsNullOrEmpty(path)) return;
			var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
			if (obj == null) return;
            
			GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(obj, Selection.activeTransform);
			
			Undo.RegisterCreatedObjectUndo(instance , $"Create {instance.name}");
			
			Selection.activeGameObject = instance;
		}
	}
}
