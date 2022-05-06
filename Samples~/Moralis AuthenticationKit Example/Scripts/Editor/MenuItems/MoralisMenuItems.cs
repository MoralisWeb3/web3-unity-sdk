using UnityEngine;
using UnityEditor;
using MoralisUnity.Sdk.Constants;
using MoralisUnity.Sdk.UI.ReadMe;

namespace MoralisUnity.Examples.Kits.Example_AuthenticationKit.MenuItems
{
	/// <summary>
	/// The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.
	/// <see cref="https://docs.unity3d.com/ScriptReference/MenuItem.html"/>
	/// </summary>
	public static class MoralisMenuItems
	{
		private const string PathMoralisExamplesWindowMenu = MoralisConstants.PathMoralisExamplesWindowMenu + "/Moralis AuthKit Examples";
		
		[MenuItem(PathMoralisExamplesWindowMenu + "/" + MoralisConstants.Open + " " + "ReadMe", false, 10 )]
		static void OpenReadMe()
		{
			ReadMeEditor.SelectReadmeGuid("299972cb71d26cb4cafdb8420e74287f");
		}
	}
}
