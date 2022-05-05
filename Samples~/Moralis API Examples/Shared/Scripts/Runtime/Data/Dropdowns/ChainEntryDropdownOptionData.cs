using MoralisUnity;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
	/// <summary>
	/// Holds data for display in <see cref="ChainsDropdown"/>
	/// </summary>
	[System.Serializable]
	public class ChainEntryDropdownOptionData : Dropdown.OptionData
	{

		//  Properties ------------------------------------
		public ChainEntry ChainEntry { get { return _chainEntry;}}
		
		//  Fields ----------------------------------------
		private ChainEntry _chainEntry = null;

		//  General Methods -------------------------------	
		public ChainEntryDropdownOptionData(ChainEntry chainEntry, string displayName): base(displayName)
		{
			_chainEntry = chainEntry;
		}
		
		//  Event Handlers --------------------------------
	}
}
