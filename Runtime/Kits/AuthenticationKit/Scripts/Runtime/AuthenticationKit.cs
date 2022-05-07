using UnityEngine;
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
    /// * Beginners : Drag-and-drop the <see cref="AuthenticationKit.prefab"/> into any Unity Scene.
    /// * Advanced : Create your own custom UI which composes the <see cref="AuthenticationKitController"/>.
    ///
    /// This <see cref="AuthenticationKit"/> class is the wrapper for the <see cref="AuthenticationKitController"/>.
    /// 
    /// </summary>
    public class AuthenticationKit : MonoBehaviour
    {
        //  Properties ------------------------------------
        public AuthenticationKitController Controller { get { return _authenticationKitController;}} 
        public bool WillAutoInitializeOnStart { get { return _willAutoInitializeOnStart;}}
        
        
        //  Fields ----------------------------------------
        [SerializeField]
        private AuthenticationKitController _authenticationKitController = new AuthenticationKitController();
        
        [Header("Settings")] 
        [SerializeField] 
        private bool _willAutoInitializeOnStart = true;

                
        //  Unity Methods ---------------------------------
        protected async void Start()
        {
            if (_willAutoInitializeOnStart)
            {
                await Controller.InitializeAsync();  
            }
        }
    }
}