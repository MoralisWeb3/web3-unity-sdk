using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using UnityEngine;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Examples.Sdk.Example_ExampleCanvas_01
{
	/// <summary>
	/// Example: Example Canvas 
	/// </summary>
	public class Example_ExampleCanvas_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton TestMeButton { get { return _exampleCanvas.Footer.Button03;}}
		
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private int _counter = 0;

		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
		}

		
		//  General Methods -------------------------------	
		private async UniTask SetupMoralis()
		{
			await Moralis.Start();
		}
		
		
		private async UniTask SetupUI()
		{
			// Canvas
			await _exampleCanvas.InitializeAsync();
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Header
			_exampleCanvas.Header.TitleText.text = "Canvas";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			_exampleCanvas.Header.AuthenticationUI.OnStateChanged.AddListener(
				AuthenticationUI_OnStateChanged);
			_exampleCanvas.Header.AuthenticationUI.OnActiveAddressChanged.AddListener(
				AuthenticationUI_OnActiveAddressChanged);
			
			// Panels
			
			// Footer
			TestMeButton.IsVisible = true;
			TestMeButton.Text.text = "TestMe Button";
			TestMeButton.Button.onClick.AddListener(TestMeButton_OnClicked);
		}

		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Top
			StringBuilder topBodyText = new StringBuilder();
			topBodyText.AppendHeaderLine($"Bold Text");
			topBodyText.AppendBullet($"Bullet Text");
			_exampleCanvas.TopPanel.BodyText.Text.text = topBodyText.ToString();
			
			// Bottom
			StringBuilder bottomBodyText = new StringBuilder();
			bottomBodyText.AppendHeaderLine($"Button Clicked");
			bottomBodyText.AppendBullet($"{_counter} Times!");
			_exampleCanvas.BottomPanel.BodyText.Text.text = bottomBodyText.ToString();

			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}


		//  Event Handlers --------------------------------
		private async void TestMeButton_OnClicked()
		{
			_counter++;
			await RefreshUI();
		}
		
		private async void AuthenticationUI_OnActiveAddressChanged(string address)
		{
			Debug.Log($"AuthenticationUI_OnActiveAddressChanged() address = {address}");
			await RefreshUI();
		}

		private async void AuthenticationUI_OnStateChanged(ExampleAuthenticationUIState state)
		{
			Debug.Log($"AuthenticationUI_OnStateChanged() state = {state}");
			await RefreshUI();
		}
	}
}
