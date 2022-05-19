using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine;

#pragma warning disable CS1998
namespace MoralisUnity.Examples.Sdk.Example_Authentication_01
{
	/// <summary>
	/// Moralis "kits" each provide drag-and-drop functionality for developers.
	/// Developers add a kit at edit-time to give additional runtime functionality for users.
	///
	/// The AuthenticationKit.prefab is designed for general use in any scene.
	///
	/// The example below uses the AuthenticationKit for the specific needs
	/// of the Moralis API Examples project.
	/// 
	/// </summary>
	public class Example_Authentication_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton BackButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		[SerializeField] 
		private AuthenticationKit _authenticationKit = null;

		private bool _wasLoggedInAtSetupMoralis = false;
		
		//  Unity Methods ---------------------------------
		protected void Awake()
		{
			_authenticationKit.OnStateChanged.AddListener(AuthenticationKit_OnStateChanged);
		}
		
		
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
		}

		
		//  General Methods -------------------------------	
		private async UniTask SetupMoralis()
		{
			Moralis.Start();
			
			_wasLoggedInAtSetupMoralis = await ExampleHelper.HasMoralisUser();
		}

		
		private async UniTask SetupUI()
		{
			// Canvas
			await _exampleCanvas.InitializeAsync();
			
			// Header
			_exampleCanvas.Header.IsVisible = false;

			// Panels
			_exampleCanvas.TopPanel.IsVisible = false;
			_exampleCanvas.TopPanel.IsInteractable = false;
			_exampleCanvas.TopPanel.IsBlockRaycasts = false;
			_exampleCanvas.BottomPanel.IsVisible = false;
			_exampleCanvas.BottomPanel.IsInteractable = false;
			_exampleCanvas.BottomPanel.IsBlockRaycasts = false;
			_exampleCanvas.BackgroundImage.IsVisible = false;
			
			// Footer
			BackButton.IsVisible = _exampleCanvas.HasSceneNamePrevious;
			BackButton.Text.text = ExampleConstants.Cancel;
			BackButton.Button.onClick.AddListener(BackButton_OnClicked);
			
		}

		private async UniTask RefreshUI()
		{
			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}

		
		//  Event Handlers --------------------------------
		private void BackButton_OnClicked()
		{
			_exampleCanvas.LoadScenePrevious();
		}
		
		private void AuthenticationKit_OnStateChanged(AuthenticationKitState authenticationKitState)
		{
			//Did you open the scene in the Unity Editor and Press Play?
			//If so, show some helpful state info
			if (_exampleCanvas.WasSceneLoadedDirectly)
			{
				Debug.Log($"State = {authenticationKitState}");
			}
			else
			{
				//Did you open ANOTHER scene in the Unity Editor and Press Play?
				//If so, this scene is designed to handle Auth more completely
				
				if (_wasLoggedInAtSetupMoralis == false && 
				    authenticationKitState == AuthenticationKitState.Connected)
				{
					// You went from NOT LOGGED to CONNECTED...
					// Success! So go back to the previous scene
					_exampleCanvas.LoadScenePrevious();
				}
				else if (_wasLoggedInAtSetupMoralis == true &&
				         authenticationKitState == AuthenticationKitState.Disconnected)
				{
					// You went from LOGGED to DISCONNECTED...
					// Success! So go back to the previous scene
					_exampleCanvas.LoadScenePrevious();
				}
			}
		}
	}
}
