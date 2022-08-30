using Cysharp.Threading.Tasks;
using MoralisUnity.SolanaApi.Models;

namespace MoralisUnity.SolanaApi.Interfaces
{
    public interface ITokenApi
    {
        Unitask<TokenPrice> GetTokenPrice(string address, NetworkTypes network);
    }
}