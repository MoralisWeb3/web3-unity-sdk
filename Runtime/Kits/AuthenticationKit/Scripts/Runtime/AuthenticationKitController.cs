using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using MoralisUnity;
using MoralisUnity.Data;
using MoralisUnity.Exceptions;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Services.ClientServices;
using UnityEngine.Events;

#pragma warning disable CS1998, CS4014
namespace MoralisUnity.Kits.AuthenticationKit
{
    /// <summary>
    /// See <see cref="AuthenticationKit"/> comments for a feature overview.
    ///
    /// This <see cref="AuthenticationKitController"/> manages state, events, and core implementation.
    /// 
    /// </summary>
    [Serializable]
    public class AuthenticationKitController 
    {
        //  Events ----------------------------------------
        
        /// <summary>
        /// Invoked upon any change to the <see cref="AuthenticationKitState"/>
        /// </summary>
        [Header("Events")]
        public AuthenticationKitStateUnityEvent OnStateChanged = new AuthenticationKitStateUnityEvent();
    
        /// <summary>
        /// Invoked when State==AuthenticationKitState.Disconnected
        /// </summary>
        public UnityEvent OnConnected = new UnityEvent();
        
        /// <summary>
        /// Invoked when State==AuthenticationKitState.Disconnected
        /// </summary>
        public UnityEvent OnDisconnected = new UnityEvent();

        //  Properties ------------------------------------
        /// <summary>
        /// Get the current <see cref="AuthenticationKitPlatform"/>
        /// </summary>
        public AuthenticationKitPlatform AuthenticationKitPlatform
        {
            get
            {
#if UNITY_ANDROID
                return AuthenticationKitPlatform.Android;
#elif UNITY_IOS
                return AuthenticationKitPlatform.iOS;
#elif UNITY_WEBGL
                return AuthenticationKitPlatform.WebGL;
#else
                return AuthenticationKitPlatform.WalletConnect;
#endif
            }
        }

        
        /// <summary>
        /// Get the current <see cref="AuthenticationKitState"/>
        /// </summary>
        public AuthenticationKitState State 
        {
            get
            {
                return _stateObservable.Value;
            }
            private set
            {
                _stateObservable.Value = value;
            }
        }

        
        //  Fields ----------------------------------------
        [Header("3rd Party")]
        [SerializeField] 
        private WalletConnect _walletConnect;
        
        private AuthenticationKitStateObservable _stateObservable = new AuthenticationKitStateObservable();

        
        //  Unity Methods ---------------------------------
        public AuthenticationKitController ()
        {
            // Any state changes here are likely too 'early'
            // to be observed externally. That is ok. Just FYI.
            State = AuthenticationKitState.PreInitialized;
            _stateObservable.OnValueChanged.AddListener(StateObservable_OnValueChanged);
        }

        
        /// <summary>
        /// Initialize the <see cref="AuthenticationKitController"/>.
        ///
        /// Calling > 1 time is indeed permitted.
        ///     * Some subsystems may be initted 1 time-per-session.
        ///     * Some subsystems may be initted >= 1 time-per-session.
        /// </summary>
        /// <returns></returns>
        public async UniTask InitializeAsync()
        {
            State = AuthenticationKitState.Initializing;

            /////////////////////////////////////////////////////////
            // TODO HACK: Remove this delay. Needed due to SDK changes in SDK v1.2.0
            await UniTask.Delay(500);
            /////////////////////////////////////////////////////////
            
            if (!Moralis.Initialized)
            {
                Moralis.Start();
            }
            
            State = AuthenticationKitState.Initialized;
            
            // If user is not logged in show the "Authenticate" button.
            if (Moralis.IsLoggedIn())
            {
                State = AuthenticationKitState.Connected;
            }
        }
        
        
#if UNITY_WEBGL
    private void FixedUpdate()
    {
        MoralisLiveQueryManager.UpdateWebSockets();
    }
#endif

        
        //  Methods ---------------------------------------

        /// <summary>
        /// Connect to Web3 session.
        /// </summary>
        public void Connect()
        {
            State = AuthenticationKitState.Connecting;
        }

        
        /// <summary>
        /// LogIn to Web3 session.
        /// </summary>
        public async UniTask LoginWithWeb3()
        {
            //TODO: This was ported from MainMenuScript.cs...The State=blah should stay here, but IMHO, move the rest of this to Moralis.cs. - samr
            
#if !UNITY_WEBGL
            new PlatformNotSupportedException();
#else
            string userAddr = "";
            if (!Web3GL.IsConnected())
            {
                await Moralis.SetupWeb3();
                userAddr = Web3GL.Account();
            }
            else
            {
                userAddr = Web3GL.Account();
            }
            
            State = AuthenticationKitState.Signing;

            if (string.IsNullOrWhiteSpace(userAddr))
            {
                Debug.LogError("Could not login or fetch account from web3.");
            }
            else 
            {
                string address = Web3GL.Account().ToLower();
                string appId = Moralis.GetClient().ApplicationId;
                long serverTime = 0;

                // Retrieve server time from Moralis Server for message signature
                Dictionary<string, object> serverTimeResponse =
                    await Moralis.GetClient().Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

                if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
                    !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
                {
                    Debug.LogError("Failed to retrieve server time from Moralis Server!");
                }

                string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";

                
                string signature = await Web3GL.Sign(signMessage);
                State = AuthenticationKitState.Signed;
                
                // Create moralis auth data from message signing response.
                Dictionary<string, object> authData = new Dictionary<string, object>
                {
                    { "id", address }, { "signature", signature }, { "data", signMessage }
                };

                // Attempt to login user.
                MoralisUser user = await Moralis.LogInAsync(authData);
                State = AuthenticationKitState.Connected;
            }
#endif
        }   
        
        
        /// <summary>
        /// LogIn to Web3 session.
        /// </summary>
#if !UNITY_WEBGL
        private async UniTask LoginViaConnectionPage()
        {
            //TODO: Is this method still needed on any platform? - samr
            MoralisUser user = await MobileLogin.LogIn(MoralisSettings.MoralisData.ServerUri,
                MoralisSettings.MoralisData.ApplicationId);

            //TODO: Can this be changed to Moralis.IsLoggedIn()???? - samr
            if (user != null)
            {
                State = AuthenticationKitState.Connected;
            }
            else
            {
                State = AuthenticationKitState.Initialized;
            }
        }
#endif
        
     
        /// <summary>
        /// Disconnect from Web3 session.
        /// </summary>
        public async void Disconnect()
        {
            State = AuthenticationKitState.Disconnecting;
            try
            {
                // Logout the Moralis User.
                await Moralis.LogOutAsync();
            }
            catch (Exception e)
            {
                // Send error to the log but not as an error as this is expected behavior from W.C.
                Debug.LogError($"Disconnect() failed. Error: {e.Message}");
            }
            
            try
            {
                // Disconnect wallet subscription.
                await _walletConnect.Session.Disconnect();
                // CLear out the session so it is re-establish on sign-in.
                _walletConnect.CLearSession();
            }
            catch (Exception e)
            {
                //Reason for Aborted warning is unknown, but expected. 
                if (e.Message != "Aborted")
                {
                    // Send error to the log but not as an error as this is expected behavior from W.C.
                    Debug.LogWarning($"[WalletConnect] Error = {e.Message}");
                }
            }
            
            // Reset the UI so its like BEFORE we ever authenticated
            await InitializeAsync();
            
            State = AuthenticationKitState.Disconnected;

        }

        
        
        //  Event Handlers --------------------------------
        private async void StateObservable_OnValueChanged( AuthenticationKitState value)
        {
            // Order matters here.
            
            // 1. Broadcast
            OnStateChanged.Invoke(_stateObservable.Value);
            
            // 2. Step the state. Rarely.
            switch (_stateObservable.Value)
            {
                case AuthenticationKitState.Connecting:
                    
                    // Safely observe
                    _walletConnect.ConnectedEventSession.RemoveListener(WalletConnect_OnConnectedEventSession);
                    _walletConnect.ConnectedEventSession.AddListener(WalletConnect_OnConnectedEventSession);
                    break;
                
                case AuthenticationKitState.Connected:
                    
                    // Unobserve
                    _walletConnect.ConnectedEventSession.RemoveListener(WalletConnect_OnConnectedEventSession);
                    
                    // Invoke redundant event
                    OnConnected.Invoke();
                    break;
                
                case AuthenticationKitState.Disconnected:
                    
                    await InitializeAsync();
                    
                    // Invoke redundant event
                    OnDisconnected.Invoke();
                    break;
                
                default:
                    break;   
            }
        }
        
        
        /// <summary>
        /// Handles when <see cref="WalletConnect"/> is connected.
        /// Here the local scope will start and finish the signing process.
        /// </summary>
        /// <param name="wcSessionData"></param>
        /// <returns></returns>
        public async void WalletConnect_OnConnectedEventSession(WCSessionData wcSessionData)
        {
            //Debug.Log($"WalletConnect_OnConnectedEventSession() wcSessionData = {wcSessionData}");
                
            State = AuthenticationKitState.Signing;
            
            // Extract wallet address from the Wallet Connect Session data object.
            string address = wcSessionData.accounts[0].ToLower();
            string appId = Moralis.GetClient().ApplicationId;
            long serverTime = 0;

            // Retrieve server time from Moralis Server for message signature
            Dictionary<string, object> serverTimeResponse = await Moralis.GetClient().Cloud
                .RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

            if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
                !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
            {
                Debug.LogError("Failed to retrieve server time from Moralis Server!"); 
            }

            string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";

            string response = await _walletConnect.Session.EthPersonalSign(address, signMessage);

            State = AuthenticationKitState.Signed;
            
            // Create moralis auth data from message signing response.
            Dictionary<string, object> authData = new Dictionary<string, object>
                { { "id", address }, { "signature", response }, { "data", signMessage } };

            // Attempt to login user.
            MoralisUser user = await Moralis.LogInAsync(authData);
            State = AuthenticationKitState.Connected;
        }
    }
}