using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using UnityEngine;


namespace MoralisUnity.Examples.Sdk.Example_MoralisClient_01
{
	/// <summary>
	/// Example: MoralisClient
	/// </summary>
	public class Example_MoralisClient_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton GetClientButton { get { return _exampleCanvas.Footer.Button03;}}

		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
			
			// Mimic user input to populate the UI
			GetClientButton_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "Client";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(4);
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"Moralis.GetClient()");
			_topBodyText.AppendBullet(
				$"Provides a way to easily interact " +
				$"with Moralis database and the Web3API");
			
			// Footer
			GetClientButton.Button.onClick.AddListener(GetClientButton_OnClicked);
			GetClientButton.Text.text = "Get Client";
			GetClientButton.IsVisible = true;
		}
		

		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();
			_exampleCanvas.BottomPanel.BodyText.ScrollToTop();

			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}


		//  Event Handlers --------------------------------
		private async void GetClientButton_OnClicked()
		{
			
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			
			// Execute
			MoralisClient moralisClient = Moralis.GetClient();
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"Moralis.GetClient()");
			_bottomBodyText.AppendLine();
			_bottomBodyText.AppendBullet($"ApplicationId = {moralisClient.ApplicationId}");
			_bottomBodyText.AppendBullet($"Cloud.ServiceHub = {moralisClient.Cloud.ServiceHub}");
			_bottomBodyText.AppendBullet($"Key = {moralisClient.Key}");
			_bottomBodyText.AppendBullet($"Session = {moralisClient.Session}");
			_bottomBodyText.AppendBullet($"EthAddress: {moralisClient.EthAddress}");
			_bottomBodyText.AppendBullet($"InstallationService: {moralisClient.InstallationService}");
			_bottomBodyText.AppendLine();
			
			// Display
			await RefreshUI();
		}
	}
}
