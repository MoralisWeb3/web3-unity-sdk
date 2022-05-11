using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine;

namespace MoralisUnity.Examples.Example_AuthenticationKitEvents
{
	/// <summary>
	/// Moralis "kits" each provide drag-and-drop functionality for developers.
	/// Developers add a kit at edit-time to give additional runtime functionality for users.
	///
	/// This <see cref="Example_AuthenticationKitEvents"/> class demonstrates usage, including
	/// the C# 'AddListener' syntax for observing events.
	/// 
	/// </summary>
	public class Example_AuthenticationKitEvents : MonoBehaviour
	{
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private AuthenticationKit _authenticationKit = null;

		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			// Optionally, The Example_AuthenticationKit can trigger
			// initialization for advanced use cases
			if (!_authenticationKit.WillInitializeOnStart)
			{
				await _authenticationKit.Controller.InitializeAsync();
			}
		}

		
		//  Event Handlers --------------------------------
		public void AuthenticationKit_OnConnected()
		{
			Debug.Log($"AuthenticationKit_OnConnected()");
		}
		
		
		public void AuthenticationKit_OnDisconnected()
		{
			Debug.Log($"AuthenticationKit_OnDisconnected()");

			// Optionally, The Example_AuthenticationKit can trigger 
			// destroy for advanced use cases
			Destroy(_authenticationKit.gameObject);
		}
	}
}