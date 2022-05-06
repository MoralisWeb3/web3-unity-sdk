using System;
using Cysharp.Threading.Tasks;
using MoralisUnity.Sdk.Exceptions;
using MoralisUnity.Sdk.Interfaces;
using MoralisUnity.Sdk.Utilities;
using UnityEngine;

namespace MoralisUnity.Examples.Sdk.Shared
{
   /// <summary>
   /// Displays "Authenticate" button or the current "0x..." address
   /// </summary>
   public class ExampleAuthenticationUI : MonoBehaviour , IInitializable
   {
      //  Events  ---------------------------------------
      [HideInInspector]
      public ExampleAuthenticationUIStateUnityEvent OnStateChanged = new ExampleAuthenticationUIStateUnityEvent();
      
      [HideInInspector]
      public StringUnityEvent OnActiveAddressChanged = new StringUnityEvent();

      //  Properties  -----------------------------------
      public ExampleButton Button { get { return _button; } }
      public bool IsInitialized { get { return _isInitialized;} }
      
      public ExampleAuthenticationUIState State
      {
         get
         {
            return _state;
         }
         private set
         {
            _state = value;
            OnStateChanged.Invoke(_state);
         }
      }
      
      public bool IsVisible
      {
         get
         {
            return _canvasGroup.GetIsVisible();
         }
         set
         {
            _canvasGroup.SetIsVisible(value);
         }
      }
      
      public bool IsInteractable
      {
         get
         {
            return _canvasGroup.interactable;
         }
         set
         {
            _canvasGroup.interactable = value;
         }
      }
      
      //Wrap API for easier use by customers in root example scripts
      public bool HasActiveAddress { get { return !string.IsNullOrEmpty(ActiveAddress);}}
      
      //Wrap API for easier use by customers in root example scripts
      public string ActiveAddress
      {
         get
         {
            if (!_isInitialized)
            {
               throw new NotInitializedException(this);
            }
            return ExampleLocalStorage.instance.ActiveAddress;
         }
         set
         {
            ExampleLocalStorage.instance.ActiveAddress = value;
         }
      }
      
      //Wrap API for easier use by customers in root example scripts
      public async UniTask<string> ResetActiveAddress()
      {
         if (!_isInitialized)
         {
            throw new NotInitializedException(this);
         }
         
         return await ExampleLocalStorage.instance.ResetActiveAddress();
      }
      
      //  Fields  ---------------------------------------
      [SerializeField] 
      private ExampleButton _button = null;
      
      [SerializeField] 
      private CanvasGroup _canvasGroup = null;
      private ExampleAuthenticationUIState _state = ExampleAuthenticationUIState.None;

      private bool _isInitialized = false;
      
      //  Unity Methods  --------------------------------

      //  General Methods  --------------------------------
      public void Initialize()
      {
         throw new Exception("This implementation requires use of InitializeAsync() instead.");
      }

      public async UniTask InitializeAsync()
      {
         if (_isInitialized)
         {
            // Some classes allow repeated calls to Initialize(),
            // But in this class - No.
            throw new AlreadyInitializedException(this);
         }
         
         ExampleLocalStorage.instance.OnActiveAddressChanged.AddListener(ExampleManager_OnActiveAddressChanged);
         OnStateChanged.AddListener(This_OnStateChanged);
         
         // Trigger refresh
         if (ExampleLocalStorage.instance.HasActiveAddress)
         {
            ExampleManager_OnActiveAddressChanged(ExampleLocalStorage.instance.ActiveAddress);
         }
         else
         {
            await ExampleLocalStorage.instance.ResetActiveAddress();
         }
         _isInitialized = true;
      }
      
      //  Event Handlers  --------------------------------
      private void ExampleManager_OnActiveAddressChanged(string address)
      {
         if (ExampleLocalStorage.instance.MoralisInterfaceIsLoggedIn())
         {
            State = ExampleAuthenticationUIState.Authenticated;
         }
         else
         {
            State = ExampleAuthenticationUIState.NotAuthenticated;
         }
         
         OnActiveAddressChanged.Invoke(address);
      }
      
      
      private void This_OnStateChanged(ExampleAuthenticationUIState state)
      {
         switch (State)
         {
            case ExampleAuthenticationUIState.Authenticated:
               _button.Text.text = Formatters.GetWeb3AddressShortFormat(ExampleLocalStorage.instance.ActiveAddress);
               break;
            case ExampleAuthenticationUIState.NotAuthenticated:
               _button.Text.text = ExampleConstants.Authenticate;
               break;
            default:
               throw new Exception("TODO: replace this with switch-exception from auth kit branch");
               break;
         }
      }
   }
}

