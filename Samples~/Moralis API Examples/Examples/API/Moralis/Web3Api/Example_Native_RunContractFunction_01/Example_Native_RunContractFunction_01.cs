using System;
using System.Text;
using System.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Sdk.Utilities;
using MoralisUnity.Web3Api.Models;
using UnityEngine;
using UnityEngine.Networking;
using UniTask = Cysharp.Threading.Tasks.UniTask;


namespace MoralisUnity.Examples.Sdk.Example_Native_RunContractFunction_01
{
	/// <summary>
	/// Example: Run Contract Function
	/// </summary>
	public class Example_Native_RunContractFunction_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton RunContractFunctionButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		
		// The contract used in this sample is setup only for mumbai
		private const ChainList _chainListRequired = ChainList.mumbai;
		
		// Conserve text space via limited results
		private LoopLimit _loopLimit = new LoopLimit(3);
		
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
			_exampleCanvas.Header.TitleText.text = "Run Contract"; //Shorter name for easy display
			_exampleCanvas.Header.ChainsDropdown.IsVisible = true;
			_exampleCanvas.Header.ChainsDropdown.OnValueChanged.AddListener(ChainsDropdown_OnValueChanged);
			_exampleCanvas.Header.ChainsDropdown.SetSelectedChain(_chainListRequired);
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(5);
		
			// Footer
			RunContractFunctionButton.IsVisible = true;
			RunContractFunctionButton.Text.text = $"Run Contract Function";
			RunContractFunctionButton.Button.onClick.AddListener(RunContractFunctionButton_OnClicked);
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
		private async void RunContractFunctionButton_OnClicked()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Prepare
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Web3Api.Native.RunContractFunction(...)");
			_topBodyText.AppendBullet($"Runs a given function of a contract abi and returns readonly data");
			_bottomBodyText.Clear();
			_bottomBodyText.AppendLine($"{ExampleConstants.Loading}");
			await RefreshUI();
			_bottomBodyText.Clear();
			
			// Execute
			string address = ExampleConstants.AddressForContractTesting; 
			string addressFormatted = Formatters.GetWeb3AddressShortFormat(address);
			MoralisClient moralisClient = Moralis.GetClient();
			ChainList chainList = _exampleCanvas.Header.ChainsDropdown.GetSelectedChain();
			
			/////////////////////////////////////////////////////////////////////////////////////////////
			// SETUP              
			
			// Function ABI input parameters
			object[] inputParams = new object[1];
			inputParams[0] = new { internalType = "uint256", name = "id", type = "uint256" };
			
			// Function ABI Output parameters
			object[] outputParams = new object[1];
			outputParams[0] = new { internalType = "string", name = "", type = "string" };
			
			// Function ABI
			object[] abi = new object[1];
			abi[0] = new { inputs = inputParams, name = "uri", outputs = outputParams, stateMutability = "view", type = "function" };

			// Define request object
			RunContractDto runContractDto = new RunContractDto()
			{
				Abi = abi,
				Params = new { id = "15310200874782" }
			};
			string functionName = "uri";

			/////////////////////////////////////////////////////////////////////////////////////////////
			_bottomBodyText.AppendHeaderLine(
				$"MoralisClient.Web3Api.Native.RunContractFunction({addressFormatted}, {functionName}, {runContractDto.Abi}, {chainList})");

			
			try
			{
				if (chainList != _chainListRequired)
				{
					throw new Exception($"Error. You must use {_chainListRequired} chain for this contract");
				}

				string result = await moralisClient.Web3Api.Native.RunContractFunction(address, functionName, runContractDto, chainList);
				
				if (!string.IsNullOrEmpty(result))
				{
					// Format the url
					string resultTrimmed = result.TrimEnd('"').TrimStart('"');
					
					// Call the url
					UnityWebRequest unityWebRequest = UnityWebRequest.Get(resultTrimmed);
					await unityWebRequest.SendWebRequest();

					// Display the url
					_bottomBodyText.AppendBullet($"result = {unityWebRequest.downloadHandler.text}");
				}
				else
				{
					_bottomBodyText.AppendBullet($"result = {result}");
				}
			}
			catch (Exception exception)
			{
				_bottomBodyText.AppendBulletException(exception);
			}
			
			// Display
			await RefreshUI();
		}
		
		private void ChainsDropdown_OnValueChanged(ChainEntry chainEntry)
		{
			RunContractFunctionButton_OnClicked();
		}
	}
}
