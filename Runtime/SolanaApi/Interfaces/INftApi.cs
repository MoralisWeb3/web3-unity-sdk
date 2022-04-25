using Cysharp.Threading.Tasks;
using MoralisUnity.SolanaApi.Models;

namespace MoralisUnity.SolanaApi.Interfaces
{
    public interface INftApi
    {
        UniTask<NftMetadata> GetNFTMetadata(NetworkTypes network, string address);
    }
}
