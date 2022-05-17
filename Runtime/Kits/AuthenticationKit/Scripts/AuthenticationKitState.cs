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
        Connecting,
        Signing,
        Signed,
        Connected,
        Disconnecting,
        Disconnected
    }
}