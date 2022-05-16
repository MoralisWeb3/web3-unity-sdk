using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Events;
using MoralisUnity.Sdk.Exceptions;

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
            _authenticationKit.OnStateChanged.AddListener(AuthenticationKit_OnStateChanged);
            
            // Local scope is 'late', so rebroadcast the state
            AuthenticationKit_OnStateChanged(_authenticationKit.State);
            
            _connectButton.onClick.AddListener(ConnectButton_OnClicked);
            _disconnectButton.onClick.AddListener(DisconnectButton_OnClicked);
        }

        protected void OnValidate()
        {
	        // This works at edit time and thus at runtime
	        // Note: At edit time the image may be NOT active (to unclutter UI). That is ok.
            if (_backgroundImageColor != null && _backgroundImage != null)
            {
                _backgroundImage.color = _backgroundImageColor;
            }
        }
        
		private void SetActiveUIJustPlatforms(bool isActive)
		{
			// 1. Hide everything...
			_iosPlatform.SetActive(false);
			_walletConnectPlatform.SetActive(false);

			if (!isActive)
			{
				return;
			}
			
			// 2. Show something...
			switch (_authenticationKit.AuthenticationKitPlatform)
			{
				case AuthenticationKitPlatform.Android:
					break;
				case AuthenticationKitPlatform.iOS:
					_iosPlatform.SetActive(true);
					break;
				case AuthenticationKitPlatform.WebGL:
					break;
				case AuthenticationKitPlatform.WalletConnect:
					_walletConnectPlatform.SetActive(true);
					break;
				default:
					SwitchDefaultException.Throw(_authenticationKit.AuthenticationKitPlatform);
					break;   
			}
		}

		
		private void SetActiveUIAllParts(bool isActive)
		{
			// Platforms
			SetActiveUIJustPlatforms(isActive);
	        
			// Buttons
			_connectButton.gameObject.SetActive(isActive);
			_disconnectButton.gameObject.SetActive(isActive);
	        
			// Texts
			_statusText.gameObject.SetActive(isActive);
			
			// Keep as default. Expected, set again after method call.
			_statusText.text = "Status"; 
		}
        
		
        //  Event Handlers --------------------------------
        private void ConnectButton_OnClicked()
        {
	        _authenticationKit.Connect();
        }
        
        
        private void DisconnectButton_OnClicked()
        {
	        _authenticationKit.Disconnect();
        }
        
        
        private void AuthenticationKit_OnStateChanged(AuthenticationKitState authenticationKitState)
        {
            switch (authenticationKitState)
            {
	            case AuthenticationKitState.None:
		            break;
                case AuthenticationKitState.PreInitialized:

	                SetActiveUIAllParts(false);
	                
                    break;
                case AuthenticationKitState.Initializing:
                    break;
                case AuthenticationKitState.Initialized:

					// Show Button "Connect"
					SetActiveUIAllParts(false);
					_connectButton.gameObject.SetActive(true);
					_backgroundImage.gameObject.SetActive(true);
            		
                    break;
                case AuthenticationKitState.Connecting:

	                // Show QR (or platform specific stuff)
	                SetActiveUIAllParts(false);
	                SetActiveUIJustPlatforms(true);
	                
	                // Show custom more text for SOME platform(s)
	                switch (_authenticationKit.AuthenticationKitPlatform)
	                {
		                case AuthenticationKitPlatform.iOS:
			                // Hide statusText first will be turned on by WalletConnect when a user click on a wallet
			                _statusText.gameObject.SetActive(false);
			                _statusText.text = "Connecting With Your Wallet";
			                // TODO show message if there is no wallet installed on the device
			                break;
		                case AuthenticationKitPlatform.Android:
			                _statusText.gameObject.SetActive(true);
			                _statusText.text = "Connecting With Your Wallet";
			                // TODO show message if there is no wallet installed on the device
			                break;
		                case AuthenticationKitPlatform.WebGL:
			                _statusText.gameObject.SetActive(true);
			                if (!Application.isEditor)
			                {
				                _statusText.text = "Connecting With Your Wallet"; 
			                }
			                else
			                {
				                // TODO Add WalletConnect option for easy play testing when developing for WebGL
				                _statusText.text = "Please build your WebGL project to connect"; 
			                }
			                break;
		                case AuthenticationKitPlatform.WalletConnect:
			                // Hide status text because the QR has it own status text
			                _statusText.gameObject.SetActive(false);
			                break;
		                default:
			                //Do nothing for other states
			                break;
	                }
	                
                    break;
                case AuthenticationKitState.Signing:
	                SetActiveUIAllParts(false);
	                _statusText.gameObject.SetActive(true);
	                _statusText.text = "Signing With Your Wallet";
                    break;
                case AuthenticationKitState.Signed:
	                SetActiveUIAllParts(false);
                    break;
                case AuthenticationKitState.Connected:
	                // Show Button "Disconnect"
					SetActiveUIAllParts(false);
        			_disconnectButton.gameObject.SetActive(true);
					break;
                case AuthenticationKitState.Disconnecting:
	                // No UI changes here
                    break;
                case AuthenticationKitState.Disconnected:
	                // No UI changes here
                    break;
                default:
	                SwitchDefaultException.Throw(authenticationKitState);
                    break;   
            }
        }
    }
}