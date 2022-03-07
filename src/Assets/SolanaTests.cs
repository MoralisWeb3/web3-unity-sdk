using Moralis.SolanaApi;
using Moralis.SolanaApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SolanaTests
{ 
    // Start is called before the first frame update
    public static async Task RunTests()
    {
       // Debug.Log("Testing Solana API via standard REST Server");
        MoralisSolanaClient.Initialize(true, "1kXrzei19HNrb3YvkLaBbOAuRo6SGcmGqmlZ2E6FYFZ2QnqO46rn3xsAX6eRMBns", "https://solana-gateway.moralis.io");

        await TestAccountApi();

        await TestNftApi();

        Debug.Log("Testing Solana API via Server Cloud Functions.");
        MoralisSolanaClient.Initialize("https://arw2wxg84h6b.moralishost.com:2053/server");

        await TestAccountApi();

        await TestNftApi();
    }

    public static async Task TestAccountApi()
    {
        try
        {
            NativeBalance bal = await MoralisSolanaClient.SolanaApi.Account.Balance(NetworkTypes.mainnet, "6XU36wCxWobLx5Rtsb58kmgAJKVYmMVqy4SHXxENAyAe");
            Debug.Log("TestAccountApi Balance Succeeded.");
        }
        catch (Exception exp)
        {
            Debug.LogError($"TestAccountApi Balance failed with {exp.Message}");
        }

        try
        {
            List<SplNft> bal = await MoralisSolanaClient.SolanaApi.Account.GetNFTs(NetworkTypes.mainnet, "6XU36wCxWobLx5Rtsb58kmgAJKVYmMVqy4SHXxENAyAe");
            Debug.Log("TestAccountApi GetNFTs Succeeded.");
        }
        catch (Exception exp)
        {
            Debug.LogError($"TestAccountApi GetNFTs failed with {exp.Message}");
        }

        try
        {
            Portfolio bal = await MoralisSolanaClient.SolanaApi.Account.GetPortfolio(NetworkTypes.mainnet, "6XU36wCxWobLx5Rtsb58kmgAJKVYmMVqy4SHXxENAyAe");
            Debug.Log("TestAccountApi GetPortfolio Succeeded.");
        }
        catch (Exception exp)
        {
            Debug.LogError($"TestAccountApi GetPortfolio failed with {exp.Message}");
        }

        try
        {
            List<SplTokenBalanace> bal = await MoralisSolanaClient.SolanaApi.Account.GetSplTokens(NetworkTypes.mainnet, "6XU36wCxWobLx5Rtsb58kmgAJKVYmMVqy4SHXxENAyAe");
            Debug.Log("TestAccountApi GetSplTokens Succeeded.");
        }
        catch (Exception exp)
        {
            Debug.LogError($"TestAccountApi GetSplTokens failed with {exp.Message}");
        }
    }

    public static async Task TestNftApi()
    {
        try
        {
            NftMetadata bal = await MoralisSolanaClient.SolanaApi.Nft.GetNFTMetadata(NetworkTypes.mainnet, "6XU36wCxWobLx5Rtsb58kmgAJKVYmMVqy4SHXxENAyAe");
            Debug.Log("TestAccountApi GetNFTMetadata Succeeded.");
        }
        catch (Exception exp)
        {
            Debug.LogError($"TestAccountApi GetNFTMetadata failed with {exp.Message}");
        }
    }
}
