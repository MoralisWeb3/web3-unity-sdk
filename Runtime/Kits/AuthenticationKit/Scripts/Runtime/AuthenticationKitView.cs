using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using System.Numerics;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System.Threading.Tasks;
using MoralisUnity.Exceptions;

namespace MoralisUnity.Kits.AuthenticationKit
{
    /// <summary>
    /// See <see cref="AuthenticationKit"/> comments for a feature overview.
    ///
    /// This <see cref="AuthenticationKitView"/> populates and observes UI elements.
    /// 
    /// </summary>
	[Serializable]
    public class AuthenticationKitView : MonoBehaviour
    {
        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        [Header("Kit")]
        [SerializeField] 
        private AuthenticationKit _authenticationKit = null;
        
        [Header("Platforms")]
        [SerializeField] 
        private GameObject _androidPlatform = null;

        [SerializeField] 
        private GameObject _iosPlatform = null;
        
        [SerializeField] 
        private GameObject _walletConnectPlatform = null;

        [Header("Buttons")]
        [SerializeField] 
        private Button _connectButton = null;

        [SerializeField] 
        private Button _disconnectButton = null;
        
        [Header("Other")]
        [SerializeField] 
        private Text _statusText = null;
        
        [SerializeField] 
        private Image _backgroundImage = null;
        
        [Header("Styling")] 
        [SerializeField] 
        private Color _backgroundImageColor = new Color(0, 0, 0, 0.5f);

        //  Unity Methods ---------------------------------
        protected void Start()
        {
            _authenticationKit.Controller.OnStateChanged.AddListener(AuthenticationKit_OnStateChanged);
            
            // Local scope is 'late', so rebroadcast the state
            AuthenticationKit_OnStateChanged(_authenticationKit.Controller.State);
            
            _connectButton.onClick.AddListener(ConnectButton_OnClicked);
            _disconnectButton.onClick.AddListener(DisconnectButton_OnClicked);
        }

        protected void OnValidate()
        {
	        //This works at edit time and thus at runtime
	        //Note: At edit time the image may be NOT active (to unclutter UI). That is ok.
            if (_backgroundImageColor != null && _backgroundImage != null)
            {
                _backgroundImage.color = _backgroundImageColor;
            }
        }

        
		private async Task ShowPlatformUI()
		{
            // If the user is still logged in just show game.
            if (!Moralis.IsLoggedIn())
            {
	            
	            // The mobile solutions for iOS and Android will be different once we
	            // smooth out the interaction with Wallet Connect. For now the duplicated 
	            // code below is on purpose just to keep the iOS and Android authentication
	            // processes separate.
	            switch (_authenticationKit.Controller.AuthenticationKitPlatform)
	            {
		            case AuthenticationKitPlatform.Android:
			            _androidPlatform.SetActive(true);
			            break;
		            case AuthenticationKitPlatform.iOS:
			            _iosPlatform.SetActive(true);
			            break;
		            case AuthenticationKitPlatform.WebGL:
			            await _authenticationKit.Controller.LoginWithWeb3();
			            break;
		            case AuthenticationKitPlatform.WalletConnect:
			            _walletConnectPlatform.SetActive(true);
			            break;
		            default:
			            SwitchDefaultException.Throw(_authenticationKit.Controller.AuthenticationKitPlatform);
			            break;   
	            }
			}
		}

		private void HidePlatformUI()
		{
			_androidPlatform.SetActive(false);
			_iosPlatform.SetActive(false);
			_walletConnectPlatform.SetActive(false);
		}
        
		
        //  Event Handlers --------------------------------
        private void ConnectButton_OnClicked()
        {
	        _authenticationKit.Controller.Connect();
        }
        
        
        private void DisconnectButton_OnClicked()
        {
	        _authenticationKit.Controller.Disconnect();
        }
        
        
        private async void AuthenticationKit_OnStateChanged(AuthenticationKitState authenticationKitState)
        {
            switch (authenticationKitState)
            {
	            case AuthenticationKitState.None:
		            break;
                case AuthenticationKitState.PreInitialized:
	                // Show Nothing
	                _walletConnectPlatform.SetActive(false);
	                _androidPlatform.SetActive(false);
	                _iosPlatform.SetActive(false);
	                _statusText.gameObject.SetActive(false);
	                _backgroundImage.gameObject.SetActive(false);
	                
	                // Show No Buttons
	                _connectButton.gameObject.SetActive(false);
	                _disconnectButton.gameObject.SetActive(false);
	                
                    break;
                case AuthenticationKitState.Initializing:
                    break;
                case AuthenticationKitState.Initialized:

					// Show Button "Connect"
					_connectButton.gameObject.SetActive(true);
         			_disconnectButton.gameObject.SetActive(false);
					_backgroundImage.gameObject.SetActive(true);
            		
                    break;
                case AuthenticationKitState.Connecting:

	                // Show No Buttons
  					_connectButton.gameObject.SetActive(false);
					_disconnectButton.gameObject.SetActive(false);

	                // Show QR (or platform specific stuff)
	                await ShowPlatformUI();
	                
	                // Show A little more text for some platform(s)
	                if (_authenticationKit.Controller.AuthenticationKitPlatform 
	                    == AuthenticationKitPlatform.WebGL)
	                {
		                _statusText.gameObject.SetActive(true);
		                _statusText.text = "Connecting";
	                }
	                else
	                {
		                _statusText.gameObject.SetActive(false);
	                }

                    break;
                case AuthenticationKitState.Signing:
	                
	                //TODO: Only for walletconnect?
	                _statusText.gameObject.SetActive(true);
	                _statusText.text = "Sign With Your Device";
	                HidePlatformUI();
	                
                    break;
                case AuthenticationKitState.Signed:
					
					_statusText.gameObject.SetActive(false);
                    break;
                case AuthenticationKitState.Connected:

					// Show Button "Disconnect"
        			_disconnectButton.gameObject.SetActive(true);
					_connectButton.gameObject.SetActive(false);
					_statusText.gameObject.SetActive(false);
					
                    break;
                case AuthenticationKitState.Disconnecting:
                    break;
                case AuthenticationKitState.Disconnected:

	                //When we disconnect, just initialize again
	                await _authenticationKit.Controller.InitializeAsync();
	                
	                //Users who care can hide the UI by listening for 
	                //AuthenticationKitState.Disconnected from game code
	                
                    break;
                default:
	                SwitchDefaultException.Throw(authenticationKitState);
                    break;   
            }
        }
    }
}