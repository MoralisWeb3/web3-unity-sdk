using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Infrastructure;
#pragma warning disable 0108
namespace Nethereum.RPC.Eth
{
    public interface IEthSyncing: IGenericRpcRequestResponseHandlerNoParam<object>
    {
#if !DOTNET35
        Task<SyncingOutput> SendRequestAsync(object id = null);
#endif
    }
}