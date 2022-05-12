using UnityEngine;
using MoralisUnity.Sdk.Exceptions;
using MoralisUnity.Sdk.Data;
using System.Collections.Generic;



namespace MoralisUnity.Kits.AuthenticationKit
{
    /// <summary>
    /// Wrapper of <see cref="AuthenticationKitState"/> which
    /// broadcasts OnValueChanged events and handles value validation
    /// </summary>
    public class AuthenticationKitStateObservable : Observable<AuthenticationKitState> 
    {
        //  Events ----------------------------------------
        
        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        
        //  Constructor Methods ---------------------------

        //  Methods ---------------------------------------
        private void ValidateNewValue(AuthenticationKitState oldValue, 
            AuthenticationKitState newValue,
            List<AuthenticationKitState> acceptableNewValues)
        {
            if (!acceptableNewValues.Contains(newValue))
            {
                throw new ObservableInvalidValueException(oldValue, newValue);
            }
        }
        
        //  Event Handlers --------------------------------
        protected override AuthenticationKitState OnValueChanging (AuthenticationKitState oldValue, 
            AuthenticationKitState newValue)
        {
            
            // Throws exceptions for any errors
            if (oldValue == AuthenticationKitState.None)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.PreInitialized
                });
            }
            
            if (oldValue == AuthenticationKitState.PreInitialized)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Initializing
                });
            }
            
            if (oldValue == AuthenticationKitState.Initializing)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Initialized
                });
            }
            
            if (oldValue == AuthenticationKitState.Initialized)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    //If not logged in
                    AuthenticationKitState.Connecting,
                    
                    //If logged in
                    AuthenticationKitState.Connected,
                    
                    //If reinitializing
                    AuthenticationKitState.Disconnected
                });
            }
            
            if (oldValue == AuthenticationKitState.Connecting)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Signing,
                    
                    // If the user cancels the connect request
                    AuthenticationKitState.Disconnecting
                });
            }
            
            if (oldValue == AuthenticationKitState.Signing)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Signed,
                    
                    // If the user cancels the signing request
                    AuthenticationKitState.Disconnecting
                });
            }
            
            if (oldValue == AuthenticationKitState.Signed)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Connected
                });
            }
            
            if (oldValue == AuthenticationKitState.Connected)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Disconnecting
                });
            }
            
            if (oldValue == AuthenticationKitState.Disconnecting)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Disconnected
                });
            }
            
            if (oldValue == AuthenticationKitState.Disconnected)
            {
                ValidateNewValue(oldValue, newValue, new List<AuthenticationKitState>
                {
                    AuthenticationKitState.Initializing
                });
            }
            
            // No errors? Good, now return new value
            return newValue;
        }
    }
}