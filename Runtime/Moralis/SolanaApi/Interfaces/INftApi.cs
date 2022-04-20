using Moralis.SolanaApi.Models;
using Cysharp.Threading.Tasks;

namespace Moralis.SolanaApi.Interfaces
{
    public interface INftApi
    {
        UniTask<NftMetadata> GetNFTMetadata(NetworkTypes network, string address);
    }
}
