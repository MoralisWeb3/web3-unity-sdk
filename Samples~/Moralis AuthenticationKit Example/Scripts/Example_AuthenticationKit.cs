using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine;

#pragma warning disable CS1998, CS4014
//TODO remove 'demo' from the name
namespace MoralisUnity.Examples.AuthenticationKitDemo
{
	/// <summary>
	/// Moralis "kits" each provide drag-and-drop functionality for developers.
	/// Developers add a kit at edit-time to give additional runtime functionality for users.
	///
	/// This <see cref="Example_AuthenticationKit"/> class demonstrates usage.
	/// 
	/// </summary>
	public class Example_AuthenticationKit : MonoBehaviour
	{
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private AuthenticationKit _authenticationKit = null;

		
		//  Unity Methods ---------------------------------
		protected void Awake()
		{
			_authenticationKit.Controller.OnStateChanged.AddListener(AuthenticationKit_OnStateChanged);
		}
		
		
		protected void Start()
		{
			// Optionally, The Example_AuthenticationKit can trigger
			// initialization for advanced use cases
			if (!_authenticationKit.WillInitializeOnStart)
			{
				_authenticationKit.Controller.InitializeAsync();
			}
		}

		
		//  Event Handlers --------------------------------
		private async void AuthenticationKit_OnStateChanged(AuthenticationKitState authenticationKitState)
		{
			//Debug.Log($"AuthenticationKit_OnStateChanged(), {authenticationKitState}");

			if (authenticationKitState == AuthenticationKitState.Disconnected)
			{
				// Optionally, The Example_AuthenticationKit can trigger 
				// destroy for advanced use cases
				Destroy(_authenticationKit.gameObject);
			}
		}
	}
}