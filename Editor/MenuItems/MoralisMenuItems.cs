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
	}
}
