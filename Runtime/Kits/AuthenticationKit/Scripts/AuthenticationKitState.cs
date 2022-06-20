using UnityEngine;

namespace MoralisUnity.Kits.AuthenticationKit
{
    /// <summary>
    /// List of possible states of the <see cref="AuthenticationKit"/>.
    /// </summary>
    public enum AuthenticationKitState
    {
        None,
        PreInitialized,
        Initializing,
        Initialized,
        WalletConnecting,
        WalletConnected,
        WalletSigning,
        WalletSigned,
        MoralisLoggingIn,
        MoralisLoggedIn,
        Disconnecting,
        Disconnected
    }
}