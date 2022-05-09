using MoralisUnity;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Platform.Queries;
using UnityEngine;


namespace MoralisUnity.Examples.Sdk.Example_Query_LiveQuery_01	
{
	/// <summary>
	/// Example: Query
	/// </summary>
	public class Example_Query_LiveQuery_01 : Example_Query_01.Example_Query_01
	{
		//  Properties ------------------------------------
		
		//  Fields ----------------------------------------

		//  Unity Methods ---------------------------------
		protected override async void Start()
		{
			///////////////////////////////////////////////
			// FYI. This Example_01_04_LiveQueries example 
			// is very similar to Example_01_04_LiveQueries 
			// so this class extends that class to simplify
			// future refactors to this example project. 
			///////////////////////////////////////////////
			await SetupMoralis();
			await SetupUI();
			
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Live Queries
			MoralisLiveQueryCallbacksForHero callbacks = new MoralisLiveQueryCallbacksForHero();
			MoralisQuery<Hero> moralisQuery = await Moralis.GetClient().Query<Hero>();
			MoralisLiveQueryController.RemoveSubscriptions("Hero");
			MoralisLiveQueryController.AddSubscription<Hero>("Hero", moralisQuery, callbacks);

			//TODO: move these into MoralisLiveQueryCallbacksForHero.cs?
			callbacks.OnCreateEvent += ((hero, requestId) =>
			{
				Debug.Log($"OnCreateEvent() Hero = {hero.Name}, level: {hero.Level}, strength: {hero.Strength} warcry: {hero.Warcry}");
			});
			callbacks.OnDeleteEvent += ((hero, requestId) =>
			{
				Debug.Log($"OnDeleteEvent() Hero = {hero.Name}.");
			});
			
			await RefreshUI();
			
			// Override the title
			ExampleCanvas.Header.TitleText.text = "Live Queries";
			
			// Mimic user input to populate the UI
			QueryHeroButton_OnClicked();
		}

		//  General Methods -------------------------------	
	}
}
