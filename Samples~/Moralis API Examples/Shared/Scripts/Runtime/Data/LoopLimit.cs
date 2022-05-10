namespace MoralisUnity.Examples.Sdk.Shared
{
	/// <summary>
	/// Concise counter to depart a foreach loop after X iterations
	/// </summary>
	[System.Serializable]
	public class LoopLimit
	{
		//  Properties ------------------------------------
		public int CountCurrent { get { return _countCurrent;}}
		public int CountMax { get { return _countMax;}}
		
		//  Fields ----------------------------------------
		private int _countCurrent = 0;
		private int _countMax = 0;
		private static readonly int CountMaxDefault = 10;

		//  General Methods -------------------------------	
		public LoopLimit(int countMax)
		{
			_countMax = countMax;
			Reset();
		}
		
		public LoopLimit()
		{
			_countMax = CountMaxDefault;
			Reset();
		}

		public bool IsAtLimit()
		{
			//Allows for first check to return false when _countMax==1. Good!
			return (++_countCurrent > _countMax);
		}
		
		public void Reset()
		{
			_countCurrent = 0;
		}
		
		//  Event Handlers --------------------------------

	}
}
