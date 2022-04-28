using MoralisUnity.Platform;
using MoralisUnity.Platform.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using MoralisUnity.Platform.Abstractions;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Services;

namespace MoralisUnity.Platform
{
    public class MoralisServiceHub : ServiceHub<MoralisUser>
    {
        public MoralisServiceHub (HttpClient httpClient, IServerConnectionData connectionData, IJsonSerializer jsonSerializer) : base(connectionData, jsonSerializer, httpClient) { }
    }
}
