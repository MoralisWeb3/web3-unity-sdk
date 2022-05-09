using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using UnityEngine;


namespace MoralisUnity.Examples.Sdk.Example_MoralisCloud_01
{
	/// <summary>
	/// Example: MoralisCloud
	/// </summary>
	public class Example_MoralisCloud_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton CallMethod01Button { get { return _exampleCanvas.Footer.Button01;}}
		private ExampleButton CallMethod02Button { get { return _exampleCanvas.Footer.Button02;}}
		private ExampleButton OpenUrlButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		[SerializeField] 
		private TextAsset _cloudFunctionsTextAsset = null;
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
			CallMethod01Button_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "Cloud Functions";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(15);
			_exampleCanvas.TopPanel.TitleText.text = "Server Side (JS)";
			_exampleCanvas.BottomPanel.TitleText.text = "Client Side Output";
			
			List<string> lines = ExampleHelper.ConvertTextAssetToLines(_cloudFunctionsTextAsset, 3);
			_topBodyText.AppendHeaderLine($"Moralis.GetClient().Cloud.RunAsync<T>(...)");
			_topBodyText.AppendBullet($"Call a method on the Moralis Cloud");
			_topBodyText.AppendHeaderLine($"{ExampleConstants.SceneSetupInstructions}");
			_topBodyText.Append("\n" + String.Join("\n", lines));
			
			// Footer
			CallMethod01Button.IsVisible = true;
			CallMethod01Button.Text.text = $"Call myMethod01";
			CallMethod01Button.Button.onClick.AddListener(CallMethod01Button_OnClicked);

			CallMethod02Button.IsVisible = true;
			CallMethod02Button.Text.text = $"Call myMethod02";
			CallMethod02Button.Button.onClick.AddListener(CallMethod02Button_OnClicked);

			OpenUrlButton.IsVisible = true;
			OpenUrlButton.Text.text = $"Open Moralis Cloud";
			OpenUrlButton.Button.onClick.AddListener(OpenUrlButton_OnClicked);
		}
		
		
		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			_exampleCanvas.TopPanel.BodyText.ScrollToTop();
			_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();
			_exampleCanvas.BottomPanel.BodyText.ScrollToTop();

			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}


		//  Event Handlers --------------------------------
		private async void CallMethod01Button_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}

			// Prepare
			_exampleCanvas.IsInteractable(false);
			
			// Execute
			string result = await Moralis.GetClient().Cloud.RunAsync<string>("myMethod01", null);

			// Display
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine ($"myMethod01 ()");
			_bottomBodyText.AppendBullet($"result = '{result}'");
			if (string.IsNullOrEmpty(result))
			{
				_bottomBodyText.AppendErrorLine($"{ExampleConstants.CloudFunctionNotFound}");
			}
			
			_exampleCanvas.IsInteractable(true);
			await RefreshUI();
		}
		
		
		private async void CallMethod02Button_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_exampleCanvas.IsInteractable(false);
			
			// Execute
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("a", 10);
			parameters.Add("b", 20);
			string result = await Moralis.GetClient().Cloud.RunAsync<string>("myMethod02", parameters);

			// Display
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine
				($"myMethod02 ({parameters["a"]}, {parameters["b"]})");

			_bottomBodyText.AppendBullet($"result = '{result}'");
			if (string.IsNullOrEmpty(result))
			{
				_bottomBodyText.AppendErrorLine($"{ExampleConstants.CloudFunctionNotFound}");
			}

			_exampleCanvas.IsInteractable(true);
			await RefreshUI();
		}
		
		
		private void OpenUrlButton_OnClicked()
		{
			Application.OpenURL(ExampleConstants.MoralisServersUrl);
		}
	}
}
