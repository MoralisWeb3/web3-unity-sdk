using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Web3Api.Models;
using UnityEngine;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Examples.Sdk.Example_ExampleChainsDropdown_01
{
	/// <summary>
	/// Example: Example Chains Dropdown 
	/// </summary>
	public class Example_ExampleChainsDropdown_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		
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
			_exampleCanvas.Header.TitleText.text = "Chains Dropdown";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = true;
			_exampleCanvas.Header.AuthenticationUI.IsVisible = false;
			_exampleCanvas.Header.ChainsDropdown.OnValueChanged.AddListener(
				ChainsDropdown_OnValueChanged);

			// Set default chain
			_exampleCanvas.Header.ChainsDropdown.SetSelectedChain(ChainList.eth);
				
			// Panels
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine("Edit-time: Use Code");
			_topBodyText.AppendLine("SetSelectedChain(ChainList.eth)");
			_topBodyText.AppendBullet("Set the selected dropdown value");
			_topBodyText.AppendHeaderLine("Runtime: Use UI");
			_topBodyText.AppendLine("Expand the ChainsDropdown UI");
			_topBodyText.AppendBullet("Set the selected dropdown value");
	
			// Footer
			_exampleCanvas.Footer.IsVisible = false;
		}

		
		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();

			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}


		//  Event Handlers --------------------------------
		private async void ChainsDropdown_OnValueChanged(ChainEntry chainEntry)
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			string displayName = ExampleHelper.GetPrettifiedNameByChainEntry(chainEntry);
			
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"ChainsDropdown_OnValueChanged()");
			_bottomBodyText.AppendBullet($"chainEntry.Name = {displayName}");
			_bottomBodyText.AppendBullet($"chainEntry.ChainId = {chainEntry.ChainId}");
			
			await RefreshUI();
		}
	}
}
