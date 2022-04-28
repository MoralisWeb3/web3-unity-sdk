using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MoralisUnity.Platform;
using MoralisUnity.Platform.Exceptions;
using MoralisUnity.Platform.Queries;
using MoralisUnity.Platform.Services;
using MoralisUnity.Platform.Services.ClientServices;
using MoralisUnity.Platform.Utilities;
using MoralisUnity.Platform.Abstractions;
using MoralisUnity.Platform.Objects;

namespace MoralisUnity.Platform
{
    public class MoralisCloud<TUser> where TUser : MoralisUser
    {
        public IServiceHub<TUser> ServiceHub;

        public MoralisCloud(IServiceHub<TUser> serviceHub) => (ServiceHub) = (serviceHub);
        public async UniTask<T> RunAsync<T>(string name, IDictionary<string, object> parameters)
        {
            MoralisUser user = await this.ServiceHub.GetCurrentUserAsync();

            T result = await this.ServiceHub.CloudFunctionService.CallFunctionAsync<T>(name, parameters, user is { } ? user.sessionToken : "");

            return result;
        }
    }
}
