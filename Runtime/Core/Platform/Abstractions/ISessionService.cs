using System.Threading;
using MoralisUnity.Platform.Objects;
using Cysharp.Threading.Tasks;

namespace MoralisUnity.Platform.Abstractions
{
    public interface ISessionService<TUser> where TUser : MoralisUser
    {
        UniTask<MoralisSession> GetSessionAsync(string sessionToken, IServiceHub<TUser> serviceHub, CancellationToken cancellationToken = default);

        UniTask RevokeAsync(string sessionToken, CancellationToken cancellationToken = default);

        UniTask<MoralisSession> UpgradeToRevocableSessionAsync(string sessionToken, IServiceHub<TUser> serviceHub, CancellationToken cancellationToken = default);

        bool IsRevocableSessionToken(string sessionToken);
    }
}
