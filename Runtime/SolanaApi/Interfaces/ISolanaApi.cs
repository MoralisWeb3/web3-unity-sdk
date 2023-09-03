﻿
namespace MoralisUnity.SolanaApi.Interfaces
{
    public interface ISolanaApi
    {
        /// <summary>
        /// AccountApi operations.
        /// </summary>
        IAccountApi Account { get; }

        /// <summary>
        /// NftApi operations.
        /// </summary>
        INftApi Nft { get; }
        
        /// <summary>
        /// TokenApi operations.
        /// </summary>
        ITokenApi Token { get; }
        
        /// <summary>
        /// Indicates that the client has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initialize the client using serverUrl. If serverUrl is null default is used. 
        /// ApiKey is passed via Configuration signleton.
        /// </summary>
        /// <param name="serverUrl"></param>
        void Initialize(string serverUrl = null);
    }
}
