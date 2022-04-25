using System.Threading;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Platform.Abstractions
{
    public interface ICurrentUserService<TUser> : ICurrentObjectService<TUser, TUser> where TUser : MoralisUser
    {
        TUser CurrentUser { get; set; }

        UniTask<string> GetCurrentSessionTokenAsync(IServiceHub<TUser> serviceHub, CancellationToken cancellationToken = default);

        UniTask LogOutAsync(IServiceHub<TUser> serviceHub, CancellationToken cancellationToken = default);
    }
}
