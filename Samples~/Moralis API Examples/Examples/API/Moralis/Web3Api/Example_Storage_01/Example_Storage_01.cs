using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using UnityEngine;
using UnityEngine.UI;


namespace MoralisUnity.Examples.Sdk.Example_Storage_01
{
	/// <summary>
	/// Example: IStorageApi
	/// </summary>
	public class Example_Storage_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton LoadImageButton { get { return _exampleCanvas.Footer.Button01;}}
		private ExampleButton SaveImageButton { get { return _exampleCanvas.Footer.Button02;}}
		private ExampleButton ClearImageButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		[SerializeField] 
		private ExamplePanel _panelForSpriteDestination = null;

		[SerializeField] 
		private Sprite _spriteToSave = null;
		
		private Sprite _spriteDestination = null;
		private List<IpfsFile> _lastLoadedIpfsFiles = new List<IpfsFile>();
		private Image _imageDestination = null;
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		private StringBuilder _bottomBodyTextError = new StringBuilder();
		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();

			// Mimic user input to populate the UI
			LoadImageButton_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "Storage";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(4);
			_bottomBodyText.Clear();
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Storage.UploadFolder(...)");
			_topBodyText.AppendBullet($"Uploads multiple files and place them in a folder directory.");
			
			// Dynamically add, for the to-be-loaded Image
			_imageDestination = ExampleHelper.CreateNewImageUnderParentAsLastSibling(
				_panelForSpriteDestination.transform.parent);
			_imageDestination.GetComponent<CanvasGroup>().SetIsVisible(false);
			
			// Footer
			LoadImageButton.IsVisible = true;
			LoadImageButton.Text.text = $"Load Image\n(Ipfs)";
			LoadImageButton.Button.onClick.AddListener(LoadImageButton_OnClicked);

			SaveImageButton.IsVisible = true;
			SaveImageButton.Text.text = $"Save Image\n(Ipfs)";
			SaveImageButton.Button.onClick.AddListener(SaveImageButton_OnClicked);

			ClearImageButton.IsVisible = true;
			ClearImageButton.Text.text = $"Clear Image\n(Ipfs)";
			ClearImageButton.Button.onClick.AddListener(ClearImageButton_OnClicked);
		}

		
		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Details
			MoralisUser moralisUser = await Moralis.GetUserAsync();

			// Text
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			if (_bottomBodyTextError.Length == 0)
			{
				_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();
			}
			else
			{
				_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyTextError.ToString();
			}
			
			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}


		private async UniTask CallUploadFolder(string content)
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Define file information.
			IpfsFileRequest ipfsFileRequest = new IpfsFileRequest()
			{
				Path = "moralis/ipfsFileRequest2.png",
				Content = content
			};
			
			// Prepare
			
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"new IpfsFileRequest()");
			_bottomBodyText.AppendBullet($"Path = {ipfsFileRequest.Path}");
			
			// Rendering text for "ipfsFileRequest.Content" is too long.
			// So use "ipfsFileRequest.Content.Length" instead
			_bottomBodyText.AppendBullet($"Content.Length = {ipfsFileRequest.Content.Length}");
			await RefreshUI();
			
			// Multiple requests can be sent via a List so define the request list.
			List<IpfsFileRequest> ipfsFileRequests = new List<IpfsFileRequest>();
			ipfsFileRequests.Add(ipfsFileRequest);

			MoralisClient moralisClient = Moralis.GetClient();

			// Cosmetic delay for UI
			_exampleCanvas.IsInteractable(false);
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();

			try
			{
				_bottomBodyTextError.Clear();
				_lastLoadedIpfsFiles = await moralisClient.Web3Api.Storage.UploadFolder(ipfsFileRequests);
			}
			catch (Exception exception)
			{
				_bottomBodyTextError.AppendErrorLine($"UploadFolder() e.Message = {exception.Message}");
				Debug.LogError($"{_bottomBodyTextError.ToString()}");
			}
			
			// Cosmetic delay for UI
			_exampleCanvas.IsInteractable(true);
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
			
		}
		
		private async void RenderImage()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			if (_lastLoadedIpfsFiles.Count >= 1 && _lastLoadedIpfsFiles[0].Path.Length > 0)
			{
				_spriteDestination = await ExampleHelper.CreateSpriteFromImageUrl(_lastLoadedIpfsFiles[0].Path);
				_imageDestination.sprite = _spriteDestination;
				_imageDestination.GetComponent<CanvasGroup>().SetIsVisible(true);
				await RefreshUI();
			}
		}
		
		
		//  Event Handlers --------------------------------
		private async void ClearImageButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"Clearing...");
			await RefreshUI();
			
			// Execute
			//The empty string will create an empty image
			string content = ""; 
			await CallUploadFolder(content);
			RenderImage();
						
			// Display
			_bottomBodyText.AppendHeaderLine($"Success!");
			await RefreshUI();
		}

		
		private async void SaveImageButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"Saving...");
			await RefreshUI();
			
			// Execute
			string content = ExampleHelper.ConvertSpriteToContentString(_spriteToSave);
			await CallUploadFolder(content);
			RenderImage();
			
			// Display
			_bottomBodyText.AppendHeaderLine($"Success!");
			await RefreshUI();
		}



		private async void LoadImageButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"Loading...");
			await RefreshUI();
			
			// Execute
			RenderImage();
			
			// Display
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"Load Image");

			if (_spriteDestination == null)
			{
				_bottomBodyText.AppendBullet($"{ExampleConstants.NothingAvailable}");
			}
			else
			{
				_bottomBodyText.AppendBullet($"Image loaded and displayed below this text");

				if (_lastLoadedIpfsFiles.Count >= 0 && _lastLoadedIpfsFiles[0].Path.Length > 0)
				{
					_bottomBodyText.AppendBullet($"Path = {_lastLoadedIpfsFiles[0].Path}");
				}
			}
			await RefreshUI();
		}
	}
}
