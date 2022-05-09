using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Platform.Objects;
using UnityEngine;


namespace MoralisUnity.Examples.Sdk.Example_MoralisUser_01	
{
	/// <summary>
	/// Example: MoralisUser
	/// </summary>
	public class Example_MoralisUser_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton GetUserButton { get { return _exampleCanvas.Footer.Button03;}}
		
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
			GetUserButton_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "User Objects";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(5);
			_topBodyText.AppendHeaderLine($"MoralisClient.GetUserAsync()");
			_topBodyText.AppendBullet(
				$"The user object contains information about the currently logged in user. " +
				$"Upon successful login, it is stored locally until logout");
			
			// Footer
			GetUserButton.IsVisible = true;
			GetUserButton.Text.text = "Get User";
			GetUserButton.Button.onClick.AddListener(GetUserButton_OnClicked);

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
		private async void GetUserButton_OnClicked()
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
			MoralisUser moralisUser = await Moralis.GetUserAsync();
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"Moralis.GetUserAsync()");
			_bottomBodyText.AppendBullet(
				$"moralisUser.email = {moralisUser.email}");
			_bottomBodyText.AppendBullet(
				$"moralisUser.password = {moralisUser.password}");
			_bottomBodyText.AppendBullet(
				$"moralisUser.username = {moralisUser.username}");
			_bottomBodyText.AppendBullet(
				$"moralisUser.sessionToken = {moralisUser.sessionToken}");
			
			// Show all contents of the authData
			_bottomBodyText.AppendBullet(
				$"moralisUser.authData...");
			foreach (KeyValuePair<string, IDictionary<string, object>> kvp in moralisUser.authData)
			{
				_bottomBodyText.AppendBullet(
					$"{kvp.Key}...", 2);
				
				if (kvp.Value != null)
				{
					foreach (var kvp2 in kvp.Value)
					{
						_bottomBodyText.AppendBullet(
							$"{kvp2.Key} = {kvp2.Value}", 3);
					}
				}
			}

			// Display
			await RefreshUI();
		}
	}
}