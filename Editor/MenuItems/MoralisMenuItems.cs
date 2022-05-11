using System.IO;
using UnityEditor;
using MoralisUnity.Sdk.Constants;
using MoralisUnity.Sdk.UI.ReadMe;
using MoralisUnity.Sdk.Utilities;

namespace MoralisUnity.Sdk.MenuItems
{
	/// <summary>
	/// The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.
	/// <see cref="https://docs.unity3d.com/ScriptReference/MenuItem.html"/>
	/// </summary>
	public static class MoralisMenuItems
	{
		[MenuItem( MoralisConstants.PathMoralisWindowMenu + "/" + MoralisConstants.OpenReadMe, false, MoralisConstants.PriorityMoralisWindow_Primary )]
		public static void OpenReadMe()
		{
			ReadMeEditor.SelectReadmeGuid("f28fe356e1effe947938cac2b8ba360a");
		}
		
		[MenuItem( MoralisConstants.PathMoralisWindowMenu + "/" + "Load Moralis Layout (10x16)", false, MoralisConstants.PriorityMoralisWindow_Secondary )]
		public static void LoadMoralisLayout()
		{
			string path = Path.GetFullPath("Packages/io.moralis.web3-unity-sdk/Editor/Layouts/MoralisUnityLayout10x16.wlt");
			UnityReflectionUtility.UnityEditor_WindowLayout_LoadWindowLayout(path);
		}
	}
}

