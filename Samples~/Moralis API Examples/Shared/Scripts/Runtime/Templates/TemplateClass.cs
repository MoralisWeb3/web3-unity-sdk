namespace MoralisUnity.Examples.Sdk.Shared.Templates
{
	/// <summary>
	/// Replace with comments...
	/// </summary>
	public class TemplateClass
	{
		//  Properties ------------------------------------
		public string SamplePublicText { get { return _samplePublicText; } set { _samplePublicText = value; }}

		
		//  Fields ----------------------------------------
		private string _samplePublicText;

		
		//  Unity Methods----------------------------------
		protected void Start()
		{

		}
		
		
		//  Initialization Methods-------------------------
		public TemplateClass()
		{

		}

		
		//  General Methods -------------------------------
		public string SamplePublicMethod(string message)
		{
			return message;
		}

		
		//  Event Handlers --------------------------------
		public void Target_OnCompleted(string message)
		{

		}
	}
}
