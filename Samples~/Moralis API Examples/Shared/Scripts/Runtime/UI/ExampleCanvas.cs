using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using MoralisUnity.Sdk.Exceptions;
using MoralisUnity.Sdk.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// This the main entry point for the UI used within every
   /// "Moralis API Examples" scene.
   ///
   /// It is designed to show 'all possible ui' and many scenes
   /// use the public API to hide unneeded elements.
   /// 
   /// </summary>
   public class ExampleCanvas : MonoBehaviour, IInitializableAsync
   {

      //  Properties  ------------------------------------
      public bool IsInitialized { get { return _isInitialized; } }
      
      /// <summary>
      /// Members can call this to ensure "Are we initialized?"
      /// </summary>
      void IInitializableAsync.RequireIsInitialized()
      {
         if (!_isInitialized)
         {
            throw new InitializationRequiredException(this);
         }
      }

      public ExampleHeader Header
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return _header;
         }
      }

      public ExampleFooter Footer
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return _footer;
         }
      }

      public ExamplePanel TopPanel
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return _panels[0];
         }
      }

      public ExamplePanel BottomPanel
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return _panels[1];
         }
      }

      public ExampleDialogSystem DialogSystem
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return _dialogSystem;
         }
      }
      
      public ExampleImage BackgroundImage
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return _backgroundImage;
         }
      }

      
      //Wrapped to hide complexity
      public bool HasSceneNamePrevious
      {
         get
         {
            (this as IInitializableAsync).RequireIsInitialized();
            return ExampleLocalStorage.instance.HasSceneNamePrevious;
         }
      }

      /// <summary>
      /// Used to detect if the activeScene was loaded DIRECTLY in Unity
      /// or if the activeScene was loaded BY another scene at RUNTIME.
      /// </summary>
      public bool WasSceneLoadedDirectly
      {
         get
         {
            // Do not require initialization before this - samr
            string authenticationSceneName = ExampleHelper.GetSceneAssetName(_authenticationSceneAsset);
            return _sceneNameLoadedDirectly == authenticationSceneName;
         }
      }


      //  Fields  ---------------------------------------
      [Header("UI")] 
      [SerializeField] 
      private Canvas _canvas = null;

      [SerializeField] 
      protected ExampleHeader _header = null;

      [SerializeField] 
      protected List<ExamplePanel> _panels = null;

      [SerializeField] 
      private ExampleFooter _footer = null;

      [SerializeField] 
      private ExampleImage _backgroundImage = null;

      [Header("Dialog")] 
      [SerializeField] 
      private ExampleDialogSystem _dialogSystem = new ExampleDialogSystem();

      [Header("Assets")] 
      [SerializeField] 
      private SceneAsset _authenticationSceneAsset = null;
      private bool _isInitialized = false;

      private static string _sceneNameLoadedDirectly = string.Empty;
      
      //  General Methods  ------------------------------
      
      [InitializeOnEnterPlayMode]
      static void OnEnterPlaymodeInEditor(EnterPlayModeOptions options)
      {
         _sceneNameLoadedDirectly = SceneManager.GetActiveScene().name;
      }
         
      /// <summary>
      /// Anything that depends on Moralis being ready, put here.
      /// </summary>
      public async UniTask InitializeAsync()
      {
         if (_isInitialized)
         {
            throw new InitializedAlreadyException(this);
         }

         // Sometimes the ExampleCanvas will be NOT active via Unity Inspector
         // For workflow ease-of-use. This ensures it IS INDEED active.
         if (gameObject.activeInHierarchy == false)
         {
            gameObject.SetActive(true);
         }
         
         // If we load directly into the AuthenticationScene. The clear out the concept of previous, 
         // which is only relevant when doing a RUNTIME change between scenes TO THE AuthenticationScene
         string authenticationSceneName = ExampleHelper.GetSceneAssetName(_authenticationSceneAsset);
         if (WasSceneLoadedDirectly && _sceneNameLoadedDirectly == authenticationSceneName)
         {
            ExampleLocalStorage.instance.ResetSceneNamePrevious();
         }
         
         // Set early in this method
         _isInitialized = true;
         
         // Default display text. (Scene's may set again, if desired)
         Header.TitleText.text = ExampleConstants.Moralis;
         Header.ChainsDropdown.Dropdown.itemText.text = ExampleConstants.Chains;
         TopPanel.TitleText.text = ExampleConstants.Main;
         BottomPanel.TitleText.text = ExampleConstants.Details;

         // Default visibility. (Scene's may set again, if desired)
         TopPanel.BodyText.Text.text = "";
         BottomPanel.BodyText.Text.text = "";
         Header.AuthenticationUI.IsVisible = true;
         Footer.Button01.IsVisible = false;
         Footer.Button02.IsVisible = false;
         Footer.Button03.IsVisible = false;

         if (!Moralis.Initialized)
         {
            await Moralis.Start();
         }
         
         // Defaults depend on IsLoggedIn...
         bool isLoggedIn = Moralis.IsLoggedIn();
         Header.ChainsDropdown.IsVisible = isLoggedIn;
         if (!isLoggedIn)
         {
            string currentSceneName = SceneManager.GetActiveScene().name;
            StringBuilder topBodyText = new StringBuilder();
            topBodyText.AppendErrorLine(ExampleConstants.YouAreNotLoggedIn);
            topBodyText.AppendLine();
            topBodyText.AppendLine(string.Format(ExampleConstants.TopPanelBodyTextMustLogInFirst1,
               authenticationSceneName, currentSceneName));
            topBodyText.AppendLine(string.Format(ExampleConstants.TopPanelBodyTextMustLogInFirst2,
               authenticationSceneName, currentSceneName));
            TopPanel.BodyText.Text.text = topBodyText.ToString();
         }
         
         // Listen
         _header.AuthenticationUI.Button.Button.onClick.AddListener(AuthenticationUI_Button_OnClicked);
         
         // Cascade Initialization
         await _header.AuthenticationUI.InitializeAsync();
         SetupChainEntryDropdown();
         
      }
      
      
      public void IsInteractable(bool isInteractable)
      {
         (this as IInitializableAsync).RequireIsInitialized();
         
         _header.ChainsDropdown.IsInteractable = isInteractable;
         _header.AuthenticationUI.IsInteractable = isInteractable;
         _footer.Button01.IsInteractable = isInteractable;
         _footer.Button02.IsInteractable = isInteractable;
         _footer.Button03.IsInteractable = isInteractable;
      }

      
      /// <summary>
      /// Adjust panel height to allow maximum number of text
      /// lines (without needing to scroll)
      /// </summary>
      /// <param name="lineCount"></param>
      public async UniTask SetMaxTextLinesForTopPanelHeight(int lineCount)
      {
         (this as IInitializableAsync).RequireIsInitialized();
         
         await SetBottomPanelHeight(ExampleConstants.GetBottomPanelHeightToLeaveTopPanelLines(lineCount));
      }
      
      
      public void LoadScenePrevious()
      {
         (this as IInitializableAsync).RequireIsInitialized();
         
         if (!HasSceneNamePrevious)
         {
            // Did you load the scene directly? Then this is expected.
            Debug.LogWarning( $"Since {SceneManager.GetActiveScene().name} was loaded directly, " +
                              $"there is no 'previous' scene. Operation cancelled. That is ok.");
            return;
         }
         
         SceneManager.LoadScene(ExampleLocalStorage.instance.SceneNamePrevious);
      }

      
      private void LoadSceneAuthentication()
      {
         ExampleLocalStorage.instance.SceneNamePrevious = SceneManager.GetActiveScene().name;
         string authenticationScenePath = ExampleHelper.GetSceneAssetPath(_authenticationSceneAsset);
         SceneManager.LoadScene(authenticationScenePath);
      }

      
      private void SetupChainEntryDropdown()
      {
         // Fetch dropdown data
         List<ChainEntry> supportedChains =
            SupportedEvmChains.SupportedChains.GetRange(0, SupportedEvmChains.SupportedChains.Count);

         // Alphabetize dropdown data
         supportedChains.Sort((a, b) => String.CompareOrdinal(a.Name, b.Name));

         // Prettify dropdown data
         List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
         foreach (ChainEntry chainEntry in supportedChains)
         {
            string displayName = ExampleHelper.GetPrettifiedNameByChainEntry(chainEntry);
            optionDatas.Add(new ChainEntryDropdownOptionData(chainEntry, displayName));
         }

         // Populate dropdown
         _header.ChainsDropdown.Dropdown.ClearOptions();
         _header.ChainsDropdown.Dropdown.AddOptions(optionDatas);

      }


      private async UniTask SetBottomPanelHeight(float nextBottomHeight)
      {
         (this as IInitializableAsync).RequireIsInitialized();
         
         // Delay so the UI triggers a rerender
         await ExampleHelper.TaskDelayWaitForCosmeticEffect();

         // There are 2 panels. Maintain the total nextBottomHeight.
         float topHeight = ExampleHelper.GetExamplePanelActualSize(TopPanel, _canvas).y;
         float bottomHeight = ExampleHelper.GetExamplePanelActualSize(BottomPanel, _canvas).y;
         float totalHeight = topHeight + bottomHeight;

         if (nextBottomHeight <= 0 || nextBottomHeight >= totalHeight)
         {
            throw new Exception($"SetBottomPanelHeight({nextBottomHeight}) with invalid argument.");
         }

         float nextTopHeight = totalHeight - nextBottomHeight;

         ExampleHelper.SetExamplePanelPreferredHeight(TopPanel, nextTopHeight);
         ExampleHelper.SetExamplePanelPreferredHeight(BottomPanel, nextBottomHeight);
      }
      

      //  Event Handlers  -------------------------------
      private void AuthenticationUI_Button_OnClicked()
      {
         ExampleAuthenticationUIState state = _header.AuthenticationUI.State;

         string authenticationSceneName= ExampleHelper.GetSceneAssetName(_authenticationSceneAsset);
         
         DialogUI dialogUI = null;
         
         switch (state)
         {
            case ExampleAuthenticationUIState.Authenticated:
               
               if (_dialogSystem.HasCurrentDialogUI)
               {
                  _dialogSystem.HideDialogBox();
               }

               ////////////////
               // SHOW DIALOG: "Edit this address..."
               ////////////////
               dialogUI = _dialogSystem.ShowDialogBoxCustomEditableText(
                  ExampleConstants.DialogTitleAddress,
                  _header.AuthenticationUI.ActiveAddress,
                  () =>
                  {
                     // Dialog Response: logout
                     LoadSceneAuthentication();
                  },
                  async () =>
                  {
                     // Dialog Response: Reset
                     string newAddress = await _header.AuthenticationUI.ResetActiveAddress();
                     dialogUI.BodyText.SetTextSafe(newAddress);
                  },
                  () =>
                  {
                     // Dialog Response: confirm
                     _header.AuthenticationUI.ActiveAddress = dialogUI.BodyText.Text.text;
                  },
                  () =>
                  {
                     // Dialog Response: cancelaction
                     // Do nothing...
                  });
               
               break;
            case ExampleAuthenticationUIState.NotAuthenticated:
               
               if (_dialogSystem.HasCurrentDialogUI)
               {
                  _dialogSystem.HideDialogBox();
               }

               string currentSceneName = SceneManager.GetActiveScene().name;

               ////////////////
               // SHOW DIALOG: "Go and log in..."
               ////////////////
               // Show 'go and log in
               dialogUI = _dialogSystem.ShowDialogBoxCustomText(
                  ExampleConstants.DialogTitleTextAuthenticate,
                  string.Format(ExampleConstants.DialogBodyTextAuthenticate, 
                     authenticationSceneName,
                     currentSceneName),
                  () =>
                  {
                     // Dialog Response: Ok
                     _dialogSystem.HideDialogBox();
                     LoadSceneAuthentication();
                  },
                  () =>
                  {
                     // Dialog Response: Cancel
                     _dialogSystem.HideDialogBox();
                  });
               
               break;
            default:
               SwitchDefaultException.Throw(state);
               break;
         }
      }
   }
}

