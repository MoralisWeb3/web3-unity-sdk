using UnityEditor;
using MoralisUnity.Sdk.Constants;
using MoralisUnity.Sdk.UI.ReadMe;

namespace MoralisUnity.Examples.Sdk.Shared
{
	/// <summary>
	/// The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.
	/// <see cref="https://docs.unity3d.com/ScriptReference/MenuItem.html"/>
	/// </summary>
	public static class MoralisMenuItems
	{
		[MenuItem( MoralisConstants.PathMoralisExamplesWindowMenu + "/Moralis API Examples" + "/" + MoralisConstants.Open + " " + "ReadMe", false, 10)]
		static void OpenReadMe()
		{
			ReadMeEditor.SelectReadmeGuid("3b4d333465945474ea57ff6e62ba4f37");
		}
	}
}
