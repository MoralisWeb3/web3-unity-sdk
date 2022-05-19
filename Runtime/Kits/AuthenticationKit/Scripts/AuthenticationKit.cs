using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Sdk.Exceptions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;

#pragma warning disable CS1998
namespace MoralisUnity.Kits.AuthenticationKit
{
    /// <summary>
    /// Moralis "kits" each provide drag-and-drop functionality for developers.
    /// Developers add a kit at edit-time to give additional runtime functionality for users.
    ///
    /// The "AuthenticationKit" provides cross-platform 
    /// support for web3 authentication (See <see cref="AuthenticationKitPlatform"/>).
    ///
    /// Usage:
    /// Drag-and-drop the <see cref="AuthenticationKit.prefab"/> into any Unity Scene.
    ///
    /// This <see cref="AuthenticationKit"/> class is the wrapper for the <see cref="AuthenticationKitController"/>.
    /// 
    /// </summary>
    public class AuthenticationKit : MonoBehaviour
    {
        //  Properties ------------------------------------
        public bool WillInitializeOnStart
        {
            get { return _willInitializeOnStart; }
        }

        [Header("Settings")] [SerializeField] private bool _willInitializeOnStart = true;

        //  Events ----------------------------------------

        /// <summary>
        /// Invoked when State==AuthenticationKitState.Connected
        /// </summary>
        [Header("Events")] public UnityEvent OnConnected = new UnityEvent();

        /// <summary>
        /// Invoked when State==AuthenticationKitState.Disconnected
        /// </summary>
        public UnityEvent OnDisconnected = new UnityEvent();

        /// <summary>
        /// Invoked upon any change to the <see cref="AuthenticationKitState"/>
        /// </summary>
        public AuthenticationKitStateUnityEvent OnStateChanged = new AuthenticationKitStateUnityEvent();

        /// <summary>
        /// Get the current <see cref="AuthenticationKitState"/>
        /// </summary>
        public AuthenticationKitState State
        {
            get { return _stateObservable.Value; }
            private set { _stateObservable.Value = value; }
        }

        private AuthenticationKitStateObservable _stateObservable = new AuthenticationKitStateObservable();

        //  Fields ----------------------------------------
        [Header("3rd Party")] [SerializeField] private WalletConnect _walletConnect;

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

        //  Unity Methods ---------------------------------
        public AuthenticationKit()
        {
            // Any state changes here are likely too 'early'
            // to be observed externally. That is ok. Just FYI.
            State = AuthenticationKitState.PreInitialized;
            _stateObservable.OnValueChanged.AddListener(StateObservable_OnValueChanged);
        }

        protected async void Start()
        {
            // Add the EventSystem if there is none
            if (FindObjectOfType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            
            if (_willInitializeOnStart)
            {
                await InitializeAsync();
            }
        }

        /// <summary>
        /// Initialize the <see cref="AuthenticationKitController"/>.
        /// </summary>
        /// <returns></returns>
        public async UniTask InitializeAsync()
        {
            State = AuthenticationKitState.Initializing;
            // Initialize Moralis
            Moralis.Start();
            State = AuthenticationKitState.Initialized;

            MoralisUser user = await Moralis.GetUserAsync();

            // If user is logged in where are connected
            if (user != null)
            {
                State = AuthenticationKitState.Connected;
            }
        }


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
#if !UNITY_WEBGL
            new PlatformNotSupportedException();
#else
            string userAddr = "";
            
            // Try to sign and catch the Exception when a user cancels the request
            try
            {
                if (!Web3GL.IsConnected())
                {
                    await Moralis.SetupWeb3();
                    userAddr = Web3GL.Account();
                }
                else
                {
                    userAddr = Web3GL.Account();
                }
            }
            catch
            {
                // Disconnect and start over if a user cancels the connecting request or there is an error
                Disconnect();
                return;
            }
            
            State = AuthenticationKitState.Signing;

            if (string.IsNullOrWhiteSpace(userAddr))
            {
                Debug.LogError("Could not login or fetch account from web3.");
            }
            else 
            {
                string address = Web3GL.Account().ToLower();
                string appId = Moralis.DappId;
                long serverTime = 0;

                // Retrieve server time from Moralis Server for message signature
                Dictionary<string, object> serverTimeResponse =
                    await Moralis.Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

                if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
                    !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
                {
                    Debug.LogError("Failed to retrieve server time from Moralis Server!");
                }

                string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";
                
                string signature = null;
                
                // Try to sign and catch the Exception when a user cancels the request
                try
                {
                    signature = await Web3GL.Sign(signMessage);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    // Disconnect and start over if a user cancels the singing request or there is an error
                    Disconnect();
                    return;
                }
                
                State = AuthenticationKitState.Signed;
                
                // Create Moralis auth data from message signing response.
                Dictionary<string, object> authData = new Dictionary<string, object>
                {
                    { "id", address }, { "signature", signature }, { "data", signMessage }
                };

                // Get chain Id
                int chainId = Web3GL.ChainId();

                // Attempt to login user.
                MoralisUser user = await Moralis.LogInAsync(authData, chainId);

                if (user != null)
                {
                    State = AuthenticationKitState.Connected;
                }
            }
#endif
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

            // WalletConnect can resume a session and trigger this event on start
            // So double check if we already go a user and are connected
            if (await Moralis.GetUserAsync() != null)
            {
                State = AuthenticationKitState.Connected;
                return;
            }

            State = AuthenticationKitState.Signing;

            // Extract wallet address from the Wallet Connect Session data object.
            string address = wcSessionData.accounts[0].ToLower();
            string appId = Moralis.DappId;
            long serverTime = 0;

            // Retrieve server time from Moralis Server for message signature
            Dictionary<string, object> serverTimeResponse = await Moralis.Cloud
                .RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

            if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
                !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
            {
                Debug.LogError("Failed to retrieve server time from Moralis Server!");
            }

            string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";

            string signature = null;

            // Try to sign and catch the Exception when a user cancels the request
            try
            {
                signature = await _walletConnect.Session.EthPersonalSign(address, signMessage);
            }
            catch
            {
                // Disconnect and start over if a user cancels the singing request or there is an error
                Disconnect();
                return;
            }

            State = AuthenticationKitState.Signed;

            // Create Moralis auth data from message signing response.
            Dictionary<string, object> authData = new Dictionary<string, object>
            {
                { "id", address }, { "signature", signature }, { "data", signMessage }
            };

            // Attempt to login user.
            MoralisUser user = await Moralis.LogInAsync(authData, wcSessionData.chainId.Value);

            if (user != null)
            {
                State = AuthenticationKitState.Connected;
            }
        }

        // If the user cancels the connect Disconnect and start over
        public async void WalletConnect_OnDisconnectedEvent(WalletConnectUnitySession session)
        {
            // Debug.Log("WalletConnect_OnDisconnectedEvent");

            // Only run if we are not already disconnecting
            if (!AuthenticationKitState.Disconnecting.Equals(State))
            {
                Disconnect();
            }
        }

        // If something goes wrong Disconnect and start over
        public async void WalletConnect_OnConnectionFailedEvent(WalletConnectUnitySession session)
        {
            // Debug.Log("WalletConnect_OnConnectionFailedEvent");

            // Only run if we are not already disconnecting
            if (!AuthenticationKitState.Disconnecting.Equals(State))
            {
                Disconnect();
            }
        }

        // If there is a new WalletConnect session setup Web3
        public async void WalletConnect_OnNewSessionConnected(WalletConnectUnitySession session)
        {
            // Debug.Log("WalletConnect_OnNewSessionConnected");

            await Moralis.SetupWeb3();
        }

        // If there is a resumed WalletConnect session setup Web3
        public async void WalletConnect_OnResumedSessionConnected(WalletConnectUnitySession session)
        {
            // Debug.Log("WalletConnect_OnResumedSessionConnected");

            await Moralis.SetupWeb3();
        }

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

#if !UNITY_WEBGL
            try
            {
                // CLear out the session so it is re-establish on sign-in.
                _walletConnect.CLearSession();

                // Disconnect the WalletConnect session
                await _walletConnect.Session.DisconnectSession("Session Disconnected", false);
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
#endif

            State = AuthenticationKitState.Disconnected;
        }

        //  Event Handlers --------------------------------
        private async void StateObservable_OnValueChanged(AuthenticationKitState value)
        {
            // Order matters here.

            // 1. Broadcast
            OnStateChanged.Invoke(_stateObservable.Value);

            // 2. Step the state. Rarely.
            switch (_stateObservable.Value)
            {
                case AuthenticationKitState.Initialized:

                    switch (AuthenticationKitPlatform)
                    {
                        case AuthenticationKitPlatform.Android:
                            _walletConnect.autoSaveAndResume = true;
                            // Warning the _walletConnect.Connect() won't finish until a Wallet connection has been established
                            await _walletConnect.Connect();
                            break;
                        case AuthenticationKitPlatform.iOS:
                            _walletConnect.autoSaveAndResume = true;
                            // Warning the _walletConnect.Connect() won't finish until a Wallet connection has been established
                            await _walletConnect.Connect();
                            break;
                    }

                    break;

                case AuthenticationKitState.Connecting:

                    switch (AuthenticationKitPlatform)
                    {
                        case AuthenticationKitPlatform.Android:
                            // Only works if a users has a app installed that handles "wc:" links
                            _walletConnect.OpenDeepLink();
                            // TODO check if the is paused with OnApplicationPause to see if the link working  
                            break;
                        case AuthenticationKitPlatform.iOS:
                            break;
                        case AuthenticationKitPlatform.WalletConnect:
                            // Warning the _walletConnect.Connect() won't finish until a Wallet connection has been established
                            await _walletConnect.Connect();
                            break;
                        case AuthenticationKitPlatform.WebGL:
                            if (!Application.isEditor)
                            {
                                await LoginWithWeb3();
                            }

                            break;
                        default:
                            SwitchDefaultException.Throw(AuthenticationKitPlatform);
                            break;
                    }

                    break;

                case AuthenticationKitState.Connected:

                    // Invoke OnConnected event
                    OnConnected.Invoke();
                    break;

                case AuthenticationKitState.Disconnected:

                    // Reset the UI so its like BEFORE we ever authenticated
                    await InitializeAsync();

                    // Invoke OnDisconnected event
                    OnDisconnected.Invoke();
                    break;

                default:
                    // Switch default is ok here since not all known conditions are declared above
                    break;
            }
        }
    }
}