using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using UnityEngine;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Examples.Sdk.Example_ExampleDialog_01
{
	/// <summary>
	/// Example: Example Dialog 
	/// </summary>
	public class Example_ExampleDialog_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton OpenDialogWithTextButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private DialogUI _dialogUI = null;
		private string _lastDialogTypeMessage = "";
		private string _lastDialogResultMessage = "";

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
			_exampleCanvas.Header.TitleText.text = "Dialog";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			_exampleCanvas.Header.AuthenticationUI.IsVisible = false;
			
			// Panels
			
			// Footer
			OpenDialogWithTextButton.IsVisible = true;
			OpenDialogWithTextButton.Text.text = "Open Dialog (Text)";
			OpenDialogWithTextButton.Button.onClick.AddListener(OpenDialogWithTextButton_OnClicked);
			
		}

		
		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Top
			StringBuilder topBodyText = new StringBuilder();
			topBodyText.AppendHeaderLine($"DialogSystem.ShowDialogBoxConfirmation");
			topBodyText.AppendBullet($"Shows a dialog window atop the Unity canvas");
			_exampleCanvas.TopPanel.BodyText.Text.text = topBodyText.ToString();
			
			// Bottom
			StringBuilder bottomBodyText = new StringBuilder();
			if (!string.IsNullOrEmpty(_lastDialogTypeMessage))
			{
				bottomBodyText.AppendHeaderLine(ExampleConstants.Type);
				bottomBodyText.AppendBullet(_lastDialogTypeMessage);
			}
			if (!string.IsNullOrEmpty(_lastDialogResultMessage))
			{
				bottomBodyText.AppendHeaderLine(ExampleConstants.Results);
				bottomBodyText.AppendBullet(_lastDialogResultMessage);
			}
			_exampleCanvas.BottomPanel.BodyText.Text.text = bottomBodyText.ToString();
			
			// Buttons
			bool hasActiveDialog =  _dialogUI != null;
			OpenDialogWithTextButton.Button.interactable = !hasActiveDialog;

			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}
		

		private async void CloseDialog(string dialogResultMessage = "")
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			_lastDialogResultMessage = dialogResultMessage;
			_exampleCanvas.DialogSystem.HideDialogBox();
			_dialogUI = null;
			await RefreshUI();
		}
		
		//  Event Handlers --------------------------------
		private async void OpenDialogWithTextButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			if (_dialogUI != null)
			{
				Debug.LogError(ExampleConstants.NotExpectedSoFix);
				return;
			}
			
			_lastDialogTypeMessage = "Confirmation";
			
			_dialogUI = _exampleCanvas.DialogSystem.ShowDialogBoxConfirmation( () =>
				{
					CloseDialog("Ok");
				},  
				() =>
				{
					CloseDialog("Cancel");
				} );
			
			await RefreshUI();
		}
		
		private async void CloseDialogButton_OnClicked()
		{
			CloseDialog();
		}
	}
}
