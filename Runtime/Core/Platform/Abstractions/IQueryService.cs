using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;

namespace MoralisUnity.Platform.Abstractions
{
    public interface IQueryService
    {
        IObjectService ObjectService { get; }
        //Task<IEnumerable<T>> FindAsync<T>(MoralisQuery<T> query, MoralisUser user, CancellationToken cancellationToken = default) where T : MoralisObject;

        //Task<IEnumerable<T>> AggregateAsync<T>(MoralisQuery<T> query, MoralisUser user, CancellationToken cancellationToken = default) where T : MoralisObject;

        //Task<int> CountAsync<T>(MoralisQuery<T> query, MoralisUser user, CancellationToken cancellationToken = default) where T : MoralisObject;

        //Task<T> FirstAsync<T>(MoralisQuery<T> query, MoralisUser user, CancellationToken cancellationToken = default) where T : MoralisObject;

        //Task<IEnumerable<T>> DistinctAsync<T>(MoralisQuery<T> query, MoralisUser user, CancellationToken cancellationToken = default) where T : MoralisObject;
        UniTask<IEnumerable<T>> FindAsync<T>(MoralisQuery<T> query, string sessionToken, CancellationToken cancellationToken = default) where T : MoralisObject;

        UniTask<IEnumerable<T>> AggregateAsync<T>(MoralisQuery<T> query, string sessionToken, CancellationToken cancellationToken = default) where T : MoralisObject;

        UniTask<int> CountAsync<T>(MoralisQuery<T> query, string sessionToken, CancellationToken cancellationToken = default) where T : MoralisObject;

        UniTask<T> FirstAsync<T>(MoralisQuery<T> query, string sessionToken, CancellationToken cancellationToken = default) where T : MoralisObject;

        UniTask<IEnumerable<T>> DistinctAsync<T>(MoralisQuery<T> query, string sessionToken, CancellationToken cancellationToken = default) where T : MoralisObject;


    }
}
