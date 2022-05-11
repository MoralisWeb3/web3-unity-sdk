using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using UnityEngine;
using WalletConnectSharp.Unity;

#pragma warning disable CS1998
namespace MoralisUnity.Examples.Sdk.Example_Sign_01	
{
	/// <summary>
	/// Example: Sign
	/// </summary>
	public class Example_Sign_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton SignButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		[SerializeField] 
		private WalletConnect _walletConnect = null;

		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		
		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
			
			// Mimic user input to populate the UI
			SignButtonButton_OnClicked();
		}

		
		//  General Methods -------------------------------	
		private async UniTask SetupMoralis()
		{
			Moralis.Start();
		}

		
		private async UniTask SetupUI()
		{
			// Canvas
			await _exampleCanvas.InitializeAsync();

			if (await ExampleHelper.HasMoralisUser() == false)
			{
				return;
			}
			
			// Header
			_exampleCanvas.Header.TitleText.text = "Signing";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = true;
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(3);
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"Session.EthPersonalSign(...)");
			_topBodyText.AppendBullet("Call signing operation");
			
			// Footer
			SignButton.IsVisible = true;
			SignButton.Text.text = $"Sign";
			SignButton.Button.onClick.AddListener(SignButtonButton_OnClicked);
		}
		
		
		private async UniTask RefreshUI()
		{
			if (await ExampleHelper.HasMoralisUser() == false)
			{
				return;
			}

			// Panels
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			_exampleCanvas.TopPanel.BodyText.ScrollToTop();
			_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();

			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}

		//  Event Handlers --------------------------------
		private async void SignButtonButton_OnClicked()
		{
			if (await ExampleHelper.HasMoralisUser() == false)
			{
				return;
			}
			
			// Prepare
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			
			// Execute
			
			// Common Sign Parameters (Needed for all signing operations)
			string address = _exampleCanvas.Header.AuthenticationUI.ActiveAddress;
			string signMessage = "My custom message";
			
			// Call
			var result = await ExampleHelper.Sign(_walletConnect, address, signMessage);
			
			// Display
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"Session.EthSign({address}, {signMessage})");
			_bottomBodyText.AppendBullet($"result = {result}");
			await RefreshUI();
		}
	}
}
