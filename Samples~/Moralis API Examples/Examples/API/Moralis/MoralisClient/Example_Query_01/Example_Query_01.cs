using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoralisUnity.Examples.Sdk.Shared;
using MoralisUnity.Platform.Queries;
using UnityEngine;


namespace MoralisUnity.Examples.Sdk.Example_Query_01	
{
	/// <summary>
	/// Example: Query
	/// </summary>
	public class Example_Query_01 : MonoBehaviour
	{
		//  Properties ------------------------------------
		private ExampleButton QueryHeroButton { get { return _exampleCanvas.Footer.Button01;}}
		private ExampleButton CreateHeroButton { get { return _exampleCanvas.Footer.Button02;}}
		private ExampleButton DeleteHeroButton { get { return _exampleCanvas.Footer.Button03;}}
		protected ExampleCanvas ExampleCanvas { get { return _exampleCanvas;}}
		
		//  Fields ----------------------------------------
		[SerializeField] 
		private ExampleCanvas _exampleCanvas = null;
		
		private StringBuilder _topBodyText = new StringBuilder();
		private StringBuilder _bottomBodyText = new StringBuilder();
		
		
		//  Unity Methods ---------------------------------
		protected virtual async void Start()
		{
			await SetupMoralis();
			await SetupUI();
			await RefreshUI();
			
			// Mimic user input to populate the UI
			QueryHeroButton_OnClicked();
		}

		
		//  General Methods -------------------------------	
		protected async UniTask SetupMoralis()
		{
			await Moralis.Start();
		}

		
		protected async UniTask SetupUI()
		{
			// Canvas
			await _exampleCanvas.InitializeAsync();
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Header
			await _exampleCanvas.InitializeAsync();
			_exampleCanvas.Header.TitleText.text = "Queries";
			_exampleCanvas.Header.ChainsDropdown.IsVisible = false;
			
			// Panels
			await _exampleCanvas.SetMaxTextLinesForTopPanelHeight(8);
			_topBodyText.Clear();
			_topBodyText.AppendHeaderLine($"MoralisClient.Create<Hero>()");
			_topBodyText.AppendBullet(
				$"Creating your own objects to support NPCs, characters, and game objects is as " +
				$"simple as creating a Plain Old C# Object (POCO)");
			
			_topBodyText.AppendHeaderLine($"MoralisClient.Query<Hero>()");
			_topBodyText.AppendBullet(
				$"Queries provide a way to retrieve information from your Moralis database");
			_topBodyText.AppendLine();
			
			// Footer
			CreateHeroButton.IsVisible = true;
			CreateHeroButton.Text.text = $"Create Hero";
			CreateHeroButton.Button.onClick.AddListener(CreateHeroButton_OnClicked);

			DeleteHeroButton.IsVisible = true;
			DeleteHeroButton.Text.text = $"Delete Hero";
			DeleteHeroButton.Button.onClick.AddListener(DeleteHeroButton_OnClicked);
			
			QueryHeroButton.IsVisible = true;
			QueryHeroButton.Text.text = $"Query Hero";
			QueryHeroButton.Button.onClick.AddListener(QueryHeroButton_OnClicked);
		}
		
		
		protected async UniTask RefreshUI()
		{
			if (!Moralis.IsLoggedIn())
			{
				return;
			}
			
			// Panels
			_exampleCanvas.TopPanel.BodyText.Text.text = _topBodyText.ToString();
			_exampleCanvas.TopPanel.BodyText.ScrollToTop();
			_exampleCanvas.BottomPanel.BodyText.Text.text = _bottomBodyText.ToString();
			_exampleCanvas.BottomPanel.BodyText.ScrollToTop();
			
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
			_bottomBodyText.AppendHeaderLine($"MoralisClient.DeleteAsync<Hero>()");
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
		
		
		protected async void QueryHeroButton_OnClicked()
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
			_bottomBodyText.AppendHeaderLine($"moralisClient.Query<Hero>()");

			if (results.Count > 0)
			{
				foreach (Hero hero in results)
				{
					_bottomBodyText.AppendBullet($"hero = {hero.objectId}");
				}
			}
			else
			{
				_bottomBodyText.AppendBullet($"{ExampleConstants.NothingAvailable}");
			}
			
			await RefreshUI();
		}
	}
}
