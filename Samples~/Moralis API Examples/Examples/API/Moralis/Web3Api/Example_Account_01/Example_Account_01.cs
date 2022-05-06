using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Sdk.Exceptions;
using MoralisUnity.Sdk.Utilities;
using MoralisUnity.Web3Api.Models;
using UnityEngine;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Examples.Sdk.Example_Account_01
{
	public enum UserOperationType
	{
		Null,
		GetNativeInfoButton,
		GetNFTInfoButton,
		GetTokenInfoButton
	}
	
	/// <summary>
	/// Example: Account
	/// </summary>
	public class Example_Account_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton GetNativeInfoButton { get { return _exampleCanvas.Footer.Button01;}}
		private ExampleButton GetNFTInfoButton { get { return _exampleCanvas.Footer.Button02;}}
		private ExampleButton GetTokenInfoButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		private StringBuilder _bottomBodyTextError = new StringBuilder();
		private UserOperationType _lastUserOperationType = UserOperationType.Null;
		
		// Conserve text space via limited results
		private LoopLimit _loopLimit = new LoopLimit(3);
		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
			
			// Mimic user input to populate the UI
			GetNativeInfoButton_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "Accounts";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = true;
			_exampleCanvas.Header.ChainsDropdown.OnValueChanged.AddListener(ChainsDropdown_OnValueChanged);
			_exampleCanvas.Header.ChainsDropdown.SetSelectedChain(ChainList.eth);
			
			// Panels
			_exampleCanvas.SetMaxTextLinesForTopPanelHeight(8);
		
			// Footer
			GetNativeInfoButton.IsVisible = true;
			GetNativeInfoButton.Text.text = $"Get Info (Native)";
			GetNativeInfoButton.Button.onClick.AddListener(GetNativeInfoButton_OnClicked);

			GetNFTInfoButton.IsVisible = true;
			GetNFTInfoButton.Text.text = $"Get Info (NFT)";
			GetNFTInfoButton.Button.onClick.AddListener(GetNFTInfoButton_OnClicked);
			
			GetTokenInfoButton.IsVisible = true;
			GetTokenInfoButton.Text.text = $"Get Info (Token)";
			GetTokenInfoButton.Button.onClick.AddListener(GetTokenInfoButton_OnClicked);
		}

		
		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			if (_bottomBodyTextError.Length == 0)
			{
				_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();
			}
			else
			{
				_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyTextError.ToString();
			}
			_exampleCanvas.BottomPanel.BodyText.ScrollToTop();
			
			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}


		//  Event Handlers --------------------------------
		private async void GetNativeInfoButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_lastUserOperationType = UserOperationType.GetNativeInfoButton;
			
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Account.GetNativeBalance(...)");
			_topBodyText.AppendBullet($"Gets native balance for a specific address");
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Account.GetTransactions(...)");
			_topBodyText.AppendBullet($"Gets native transactions in descending order based on block number");
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			_bottomBodyText.Clear();
			
			// Execute
			string address = ExampleConstants.AddressForTesting; // OPTIONAL: Use Value From UI: _exampleCanvas.Header.AuthenticationUI.ActiveAddress
			string addressFormatted = Formatters.GetWeb3AddressShortFormat(address);
			MoralisClient moralisClient = Moralis.GetClient();
			ChainList chainList = _exampleCanvas.Header.ChainsDropdown.GetSelectedChain();
			
			// GetNativeBalance
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetNativeBalance({addressFormatted}, {chainList})");
			
			try
			{
				NativeBalance nativeBalance  = 
					await moralisClient.Web3Api.Account.GetNativeBalance(address, chainList);
				_bottomBodyText.AppendBullet($"nativeBalance.Balance = {nativeBalance.Balance}");
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// GetTransactions
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetTransactions({addressFormatted}, {chainList})");
			
			try
			{
				TransactionCollection transactionCollection  = 
					await moralisClient.Web3Api.Account.GetTransactions(address, chainList);
				_bottomBodyText.AppendBullet($"transactionCollection.Result.Count = {transactionCollection.Result.Count}");
				
				_loopLimit.Reset();
				_bottomBodyText.AppendBulletLoopLimit(_loopLimit);
				foreach (Transaction transaction in transactionCollection.Result)
				{
					if (_loopLimit.IsAtLimit())
					{
						break;
					}
					_bottomBodyText.AppendBullet($"transaction.Value = {transaction.Value}", 2);
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// Display
			await RefreshUI();
		}
		
		
		private async void GetTokenInfoButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_lastUserOperationType = UserOperationType.GetTokenInfoButton;
			
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Account.GetTokenBalances(...)");
			_topBodyText.AppendBullet($"Gets token balances for a specific address");
			_topBodyText.AppendHeaderLine( $"MoralisClient.Web3Api.Account.GetTokenTransfers(...)");
			_topBodyText.AppendBullet($"Gets ERC20 token transactions in descending order based on block number");
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			_bottomBodyText.Clear();
			
			// Execute
			string address = ExampleConstants.AddressForTesting; // TODO: Use Moralis.GetUser().ethAddress;
			string addressFormatted = Formatters.GetWeb3AddressShortFormat(address);
			MoralisClient moralisClient = Moralis.GetClient();
			ChainList chainList = _exampleCanvas.Header.ChainsDropdown.GetSelectedChain();
			
			// GetTokenBalances
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetTokenBalances({addressFormatted}, {chainList})");
			try
			{
				List<Erc20TokenBalance> tokenBalances = 
					await moralisClient.Web3Api.Account.GetTokenBalances(address, chainList);
				_bottomBodyText.AppendBullet($"tokenBalances.Count = {tokenBalances.Count}", 1);

				_loopLimit.Reset();
				_bottomBodyText.AppendBulletLoopLimit(_loopLimit);
				foreach (Erc20TokenBalance tokenBalance in tokenBalances)
				{
					if (_loopLimit.IsAtLimit())
					{
						break;
					}
					_bottomBodyText.AppendBullet($"tokenBalance = {tokenBalance.Balance}", 2);
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// GetTokenTransfers
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetTokenTransfers({addressFormatted}, {chainList})");
			try
			{
				Erc20TransactionCollection erc20TransactionCollection = 
					await moralisClient.Web3Api.Account.GetTokenTransfers(address, chainList);
				_bottomBodyText.AppendBullet($"erc20TransactionCollection.Result.Count = {erc20TransactionCollection.Result.Count}", 1);

				_loopLimit.Reset();
				_bottomBodyText.AppendBulletLoopLimit(_loopLimit);
				foreach (Erc20Transaction erc20Transaction in erc20TransactionCollection.Result)
				{
					if (_loopLimit.IsAtLimit())
					{
						break;
					}
					string ercAddressFormatted = Formatters.GetWeb3AddressShortFormat(erc20Transaction.Address);
					_bottomBodyText.AppendBullet($"erc20Transaction.Address = {ercAddressFormatted}", 2);
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// Display
			await RefreshUI();
		}
		
		
		private async void GetNFTInfoButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_lastUserOperationType = UserOperationType.GetNFTInfoButton;
			
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Account.GetNFTs(...)");
			_topBodyText.AppendBullet($"Gets NFTs owned by the given address");
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Account.GetNFTTransfers(...)");
			_topBodyText.AppendBullet($"Gets the transfers of the tokens matching the given parameters");
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Account.GetNFTsForContract(...)");
			_topBodyText.AppendBullet($"Gets NFTs owned by the given address");
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			_bottomBodyText.Clear();
			
			// Execute
			string address = ExampleConstants.AddressForTesting; // TODO: Use Moralis.GetUser().ethAddress;
			string addressFormatted = Formatters.GetWeb3AddressShortFormat(address);
			MoralisClient moralisClient = Moralis.GetClient();
			ChainList chainList = _exampleCanvas.Header.ChainsDropdown.GetSelectedChain();
			
			// GetNFTs
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetNFTs({addressFormatted}, {chainList})");
			
			try
			{
				NftOwnerCollection nftOwnerCollection = 
					await moralisClient.Web3Api.Account.GetNFTs(address, chainList);
				_bottomBodyText.AppendBullet(
					$"nftOwnerCollection.Result.Count = {nftOwnerCollection.Result.Count}", 1);
				
				_loopLimit.Reset();
				_bottomBodyText.AppendBulletLoopLimit(_loopLimit);
				foreach (NftOwner nftOwner in nftOwnerCollection.Result)
				{
					if (_loopLimit.IsAtLimit())
					{
						break;
					}
					_bottomBodyText.AppendBullet($"nftOwner.Name = {nftOwner.Name}", 2);
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// GetNFTTransfers
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetNFTTransfers({addressFormatted}, {chainList})");
			
			try
			{
				NftTransferCollection nftTransferCollection = 
					await moralisClient.Web3Api.Account.GetNFTTransfers(address, chainList);
				_bottomBodyText.AppendBullet(
					$"nftTransferCollection.Result.Count = {nftTransferCollection.Result.Count}", 1);
				
				_loopLimit.Reset();
				_bottomBodyText.AppendBulletLoopLimit(_loopLimit);
				foreach (NftTransfer nftTransfer in nftTransferCollection.Result)
				{
					if (_loopLimit.IsAtLimit())
					{
						break;
					}
					_bottomBodyText.AppendBullet($"nftTransfer.Amount = {nftTransfer.Amount}", 2);
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
				
			// GetNFTTransfers
			string tokenAddress = ExampleConstants.TokenAddressForTesting;
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Account.GetNFTsForContract({addressFormatted}, {tokenAddress}, {chainList})");
			
			try
			{
				NftOwnerCollection nftOwnerCollection = 
					await moralisClient.Web3Api.Account.GetNFTsForContract(address, tokenAddress , chainList);
				_bottomBodyText.AppendBullet(
					$"nftOwnerCollection.Result.Count = {nftOwnerCollection.Result.Count}", 1);
				
				_loopLimit.Reset();
				_bottomBodyText.AppendBulletLoopLimit(_loopLimit);
				foreach (NftOwner nftOwner in nftOwnerCollection.Result)
				{
					if (_loopLimit.IsAtLimit())
					{
						break;
					}
					_bottomBodyText.AppendBullet($"nftOwner.Amount = {nftOwner.Amount}", 2);
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// Display
			await RefreshUI();
		}
		
		
		private async void ChainsDropdown_OnValueChanged(ChainEntry chainEntry)
		{
			// Replay the last button click whenever dropdown changes
			switch (_lastUserOperationType)
			{
				case UserOperationType.GetNativeInfoButton:
					GetNativeInfoButton_OnClicked();
					break;
				case UserOperationType.GetNFTInfoButton:
					GetNFTInfoButton_OnClicked();
					break;
				case UserOperationType.GetTokenInfoButton:
					GetTokenInfoButton_OnClicked();
					break;
				default:
					SwitchDefaultException.Throw(_lastUserOperationType);
					break;
			}
			
			await RefreshUI();
		}
	}
}
