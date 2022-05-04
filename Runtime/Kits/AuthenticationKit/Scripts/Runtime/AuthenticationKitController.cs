using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using UnityEngine.Events;
using MoralisUnity;
using MoralisUnity.Exceptions;
using MoralisUnity.Platform.Objects;
using UnityEditor.VersionControl;
using System;

#if UNITY_WEBGL
#else
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
#endif

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
        public AuthenticationKitStateUnityEvent OnStateChanged = new AuthenticationKitStateUnityEvent();
    
        //  Properties ------------------------------------
       
        public AuthenticationKitState State 
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


        //  Fields ----------------------------------------
        [Header("3rd Party")]
        [SerializeField] 
        private WalletConnect _walletConnect;

        private AuthenticationKitState _state = AuthenticationKitState.None;
        
        //  Unity Methods ---------------------------------
        public AuthenticationKitController ()
        {
            State = AuthenticationKitState.PreInitialized;
        }

        /// <summary>
        /// IMPORTANT!!!
        /// Some things WITHIN may be only once-per-session.
        /// Some things WITHIN may be more than once-per-session.
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
            
            //TODO: Add Moralis.Start() somewhere
            //TODO: Remove/refactor the webgl stuff in this class?
            //await InitializeMoralisInterfaceInitializer();
            State = AuthenticationKitState.Initialized;
            
            //unlisten, then listen
            _walletConnect.ConnectedEventSession.RemoveListener(WalletConnectHandler);
            _walletConnect.ConnectedEventSession.AddListener(WalletConnectHandler);
            
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
        /// Used to start the authentication process and then run the game. If the 
        /// user has a valid Moralis session thes user is redirected to the next 
        /// scene.
        /// </summary>
        public void Connect()
        {
            //Limited validation
            if (State != AuthenticationKitState.Initialized)
            {
                throw new UnexpectedStateException(State, AuthenticationKitState.Initialized);
            }
            State = AuthenticationKitState.Connecting;
        }

        /// <summary>
        /// Login using web3
        /// </summary>
        /// <returns></returns>
        public async UniTask LoginWithWeb3()
        {
            
#if !UNITY_WEBGL
            // Codepath calling LoginWithWeb3 yet WEBGL is not available per #ifdef
            new PlatformNotSupportedException();
#else
            string userAddr = "";
            if (!Web3GL.IsConnected())
            {
                userAddr = await MoralisInterface.SetupWeb3();
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
                string appId = MoralisInterface.GetClient().ApplicationId;
                long serverTime = 0;

                // Retrieve server time from Moralis Server for message signature
                Dictionary<string, object> serverTimeResponse =
                    await MoralisInterface.GetClient().Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

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
                MoralisUser user = await MoralisInterface.LogInAsync(authData);
                State = AuthenticationKitState.Connected;
            }
#endif
        }   
        
        /// <summary>
        /// Display Moralis connector login page
        /// </summary>
#if !UNITY_WEBGL
        private async UniTask LoginViaConnectionPage()
        {
            //TODO: Is this method still needed on any platform? - samr
            // Use Moralis Connect page for authentication as we work to make the Wallet 
            // Connect experience better.
            MoralisUser user = await MobileLogin.LogIn(MoralisSettings.MoralisData.ServerUri,
                MoralisSettings.MoralisData.ApplicationId);

            //TODO: Can this be changed to MoralisInterface.IsLoggedIn()????
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
        /// Closeout connections
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
        
        /// <summary>
        /// Handles the Wallet Connect OnConnection event.
        /// When user grants wallet connection to application this 
        /// method is called. A request for signature is sent to wallet. 
        /// If users agrees to sign the message the signed message is 
        /// used to authenticate with Moralis.
        /// </summary>
        /// <param name="data">WCSessionData</param>
        public async void WalletConnectHandler(WCSessionData data)
        {
            State = AuthenticationKitState.Signing;
            
            // Extract wallet address from the Wallet Connect Session data object.
            string address = data.accounts[0].ToLower();
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