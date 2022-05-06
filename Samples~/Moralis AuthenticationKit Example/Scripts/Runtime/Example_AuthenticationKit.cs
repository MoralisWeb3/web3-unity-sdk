using MoralisUnity.Kits.AuthenticationKit;
using MoralisUnity.Sdk.Data;
using UnityEngine;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Examples.Kits.Example_AuthenticationKit
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
		//  Properties ------------------------------------

		
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
			// Optionally, The demo itself can trigger init
			// For advanced use cases
			if (!_authenticationKit.WillAutoInitializeOnStart)
			{
				_authenticationKit.Controller.InitializeAsync();
				_authenticationKit.Controller.Connect();
			}


		}
		
		//  Event Handlers --------------------------------
		private void AuthenticationKit_OnStateChanged(AuthenticationKitState authenticationKitState)
		{
			//Debug.Log($"AuthenticationKit_OnStateChanged(), {authenticationKitState}");

			if (authenticationKitState == AuthenticationKitState.Disconnected)
			{
				// Optionally, The demo itself can trigger destroy
				// For advanced use cases
				Destroy(_authenticationKit.gameObject);
			}
		}
	}
}