using MoralisUnity.Platform.Abstractions;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Services.ClientServices;

namespace MoralisUnity.Platform.Queries
{
    public static class QueryExtensions
    {
        public static MoralisLiveQueryClient<T> Subscribe<T>(this MoralisQuery<T> query, ILiveQueryCallbacks<T> callbacks = null) where T : MoralisObject
        {
            if (!(callbacks is { }))
            {
                callbacks = new LiveQueryCallbacks<T>();
            }

            string sessionToken = query.SessionToken;
            string installationId = query.InstallationService.InstallationId?.ToString();
            
            MoralisLiveQueryClient<T>  subscription = 
                MoralisLiveQueryManager.CreateSubscription<T>(query, query.ServerConnectionData, callbacks, query.JsonSerializer, sessionToken, installationId);

            return subscription;
        }
    }
}