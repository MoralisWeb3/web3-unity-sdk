using System;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Sdk.Utilities;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using UnityEngine;


namespace MoralisUnity.Examples.Sdk.Example_Eth_TransactionManager_01	
{
	/// <summary>
	/// Example: ITransactionManager
	/// </summary>
	public class Example_Eth_TransactionManager_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton TransferButton { get { return _exampleCanvas.Footer.Button03;}}
		
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
			TransferButton_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "Transferring";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			_exampleCanvas.Header.AuthenticationUI.OnActiveAddressChanged.AddListener(
				AuthenticationUI_OnActiveAddressChanged);
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(5);
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine(
				$"Moralis.Web3Client.Eth.TransactionManager.SendTransactionAsync(...)");
			_topBodyText.AppendBullet(
				$"Transfer some value of balance from one address to another address");
			
			// Footer
			TransferButton.IsVisible = true;
			TransferButton.Text.text = $"Transfer";
			TransferButton.Button.onClick.AddListener(TransferButton_OnClicked);
		}
		
		
		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
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

		
		/// <summary>
		/// Do series of checks and build error logs
		/// </summary>
		/// <param name="transactionInput"></param>
		/// <returns></returns>
		private bool IsValidTransactionInput (TransactionInput transactionInput)
		{
			bool isValid1 = transactionInput.Value.Value > 0;
			if (!isValid1)
			{
				_bottomBodyText.AppendBulletError($"Failed! Invalid parameter. amountToSendWei = {transactionInput.Value}");
			}
		
			bool isValid2 = Validators.IsValidWeb3AddressFormat(transactionInput.From);
			if (!isValid2)
			{
				_bottomBodyText.AppendBulletError($"Failed! Invalid parameter. fromAddress = {transactionInput.From}");
			}

			bool isValid3 = Validators.IsValidWeb3AddressFormat(transactionInput.To);
			if (!isValid3)
			{
				_bottomBodyText.AppendBulletError($"Failed! Invalid parameter. toAddress = {transactionInput.To}");
			}

			bool isValid4 = transactionInput.From != transactionInput.To;
			if (!isValid4)
			{
				_bottomBodyText.AppendBulletError(
					$"Failed! fromAddress must not equal toAddress. Hardcode new value in {this.GetType().Name}.cs");
			}

			return isValid1 && isValid2 && isValid3 && isValid4;
		}
		
		//  Event Handlers --------------------------------
		private void AuthenticationUI_OnActiveAddressChanged(string address)
		{
			TransferButton_OnClicked();
		}
		
		private async void TransferButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			_bottomBodyText.Clear();
			
			// Logging ignores the live parameter value to save space
			_bottomBodyText.AppendHeaderLine(
				$"Moralis.Web3Client.Eth.TransactionManager.SendTransactionAsync(...)");
			
			// Execute
			float amountToSendNative = 1;
			var amountToSendWei = UnitConversion.Convert.ToWei(amountToSendNative);
			string fromAddress = _exampleCanvas.Header.AuthenticationUI.ActiveAddress;
			
			// User Must: Hardcode a valid toAddress here...
			string toAddress = fromAddress; 
			
			// Validation
			TransactionInput transactionInput = new TransactionInput()
			{
				Data = String.Empty,
				From = fromAddress,
				To = toAddress,
				Value = new HexBigInteger(amountToSendWei)
			};
			bool isValidTransactionInput = IsValidTransactionInput(transactionInput);

			// Call SendTransactionAsync
			if (isValidTransactionInput)
			{

#if UNITY_WEBGL
				throw new PlatformNotSupportedException();
#else
				try
				{
					string result = await Moralis.Web3Client.Eth.TransactionManager.SendTransactionAsync(transactionInput);
					_bottomBodyText.AppendBullet($"Success! Transferred {amountToSendWei} " +
					                             $"WEI from {fromAddress} to {toAddress}, result = {result}");
				}
				catch (Exception exception)
				{
					_bottomBodyText.AppendBulletError($"Failed! Transfer of {amountToSendWei} " +
					                                  $"WEI from {fromAddress} to {toAddress}, error = {exception.Message}");
				}
#endif


			}
			
			// Display
			await RefreshUI();
		}
	}
}
