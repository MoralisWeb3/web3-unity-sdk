using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Platform.Queries;
using UnityEngine;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Examples.Sdk.Example_CustomObjects_01
{
	/// <summary>
	/// Example: Custom Objects
	/// </summary>
	public class Example_CustomObjects_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton CreateHeroButton { get { return _exampleCanvas.Footer.Button02;}}
		private ExampleButton DeleteHeroButton { get { return _exampleCanvas.Footer.Button03;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		
		
		//  Unity Methods ---------------------------------
		protected async void Start()
		{
			///////////////////////////////////////////////
			// FYI. This Example_01_05_CustomObjects example 
			// is very similar to Example_01_03_Queries 
			// so this class is best copy/pasted from there
			// and slightly modified when this project is updated. 
			///////////////////////////////////////////////
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
						
			// Mimic user input to populate the UI
			CreateHeroButton_OnClicked();
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
			_exampleCanvas.Header.TitleText.text = "Custom Objects";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			
			// Panels
			_exampleCanvas.SetMaxTextLinesForTopPanelHeight(7);
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Create<Hero>()");
			_topBodyText.AppendBullet(
				$"Creating your own objects to support NPCs, characters, and game objects is as " +
				$"simple as creating a Plain Old C# Object (POCO)");
			
			_topBodyText.AppendHeaderLine($"MoralisClient.DeleteAsync(hero)");
			_topBodyText.AppendBullet(
				$"Destroy an existing custom object");
			_topBodyText.AppendLine();
			
			// Footer
			CreateHeroButton.IsVisible = true;
			CreateHeroButton.Text.text = $"Create Hero";
			CreateHeroButton.Button.onClick.AddListener(CreateHeroButton_OnClicked);

			DeleteHeroButton.IsVisible = true;
			DeleteHeroButton.Text.text = $"Delete Hero";
			DeleteHeroButton.Button.onClick.AddListener(DeleteHeroButton_OnClicked);

		}
		

		private async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Panels
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();
			
			// Cosmetic delay for UI
			await ExampleHelper.TaskDelayWaitForCosmeticEffect();
		}

		
		//  Event Handlers --------------------------------
		private async void CreateHeroButton_OnClicked()
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
			Hero hero = moralisClient.Create<Hero>();
			hero.Name = "Zuko";
			hero.Strength = 50;
			hero.Level = 15;
			hero.Warcry = "Honor!!!";
			hero.Bag.Add("Leather Armor");
			hero.Bag.Add("Crown Prince Hair clip.");
			await hero.SaveAsync();
			
			// Display
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"moralisClient.Create<Hero>()");
			_bottomBodyText.AppendBullet($"hero.Name = {hero.Name}");
			
			await RefreshUI();
		}
		
		private async void DeleteHeroButton_OnClicked()
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
			MoralisQuery<Hero> moralisQuery1 = await moralisClient.Query<Hero>();
			MoralisQuery<Hero> moralisQuery2 =  moralisQuery1.WhereEqualTo("Level", 15);
			
			IEnumerable<Hero> result = await moralisQuery2.FindAsync();
			List<Hero> results = result.ToList();

			// Display
			_bottomBodyText.Clear();
			_bottomBodyText.AppendHeaderLine($"MoralisClient.DeleteAsync(hero)");
			if (results.Count > 0)
			{
				Hero heroToDelete = results[0];
				_bottomBodyText.AppendBullet($"heroToDelete = {heroToDelete.objectId}");
				await moralisClient.DeleteAsync(heroToDelete);
			}
			else
			{
				_bottomBodyText.AppendBullet($"{ExampleConstants.NothingAvailable}");
			}

			await RefreshUI();
		}
	}
}
