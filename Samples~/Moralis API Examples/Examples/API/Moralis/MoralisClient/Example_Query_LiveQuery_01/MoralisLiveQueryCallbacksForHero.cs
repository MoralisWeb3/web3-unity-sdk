using MoralisUnity.Examples.Sdk.Shared;


namespace MoralisUnity.Examples.Sdk.Example_Query_LiveQuery_01	
{
	/// <summary>
	/// Example: Live Query Callbacks 
	/// </summary>
	public class MoralisLiveQueryCallbacksForHero : MoralisLiveQueryCallbacks<Hero>
	{

		//  Properties ------------------------------------
		
		//  Fields ----------------------------------------
		private const string Title = "Callbacks";
		
		//  Constructor Methods ---------------------------
		public MoralisLiveQueryCallbacksForHero()
		{
			// Here is example syntax for more callbacks
			
			/*
			OnConnectedEvent += (() => { Debug.Log($"{Title}: Connection Established."); });
			OnSubscribedEvent += ((requestId) => { Debug.Log($"{Title}: Subscription {requestId} created."); });
			OnUnsubscribedEvent += ((requestId) => { Debug.Log($"{Title}: Unsubscribed from {requestId}."); });
			OnErrorEvent += ((ErrorMessage em) =>
			{
				Debug.Log($"{Title}: ERROR: code: {em.code}, msg: {em.error}, requestId: {em.requestId}");
			});
			OnCreateEvent += ((item, requestId) =>
			{
				Debug.Log($"{Title}: Created hero: name: {item.Name}, level: {item.Level}, strength: {item.Strength} warcry: {item.Warcry}");
			});
			OnUpdateEvent += ((item, requestId) =>
			{
				Debug.Log($"{Title}: Updated hero: name: {item.Name}, level: {item.Level}, strength: {item.Strength} warcry: {item.Warcry}");
			});
			OnDeleteEvent += ((item, requestId) =>
			{
				Debug.Log($"{Title}: Hero {item.Name} has been defeated and removed from the roll!");
			});
			OnGeneralMessageEvent += ((text) =>
			{
				//NOTE: This logging happens often - srivello
				//Debug.Log($"{Title}: Websocket message: {text}");
			});
			*/
			
		}

		//  General Methods -------------------------------	
		
		//  Event Handlers --------------------------------
	}
}
