using System.Collections.Generic;
using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Examples.Sdk.Shared
{
	/// <summary>
	/// Example: Moralis Object
	/// </summary>
	public class Hero : MoralisObject
	{
		//  Properties ------------------------------------

		//  Fields ----------------------------------------
		public int Strength { get; set; }
		public int Level { get; set; }
		public string Name { get; set; }
		public string Warcry { get; set; }
		public List<string> Bag { get; set; }

		//  Initialization Methods ---------------------------------
		public Hero() : base("Hero") 
		{
			Bag = new List<string>();
		}
		
		//  General Methods -------------------------------	


		//  Event Handlers --------------------------------
	}
}
