using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace MoralisUnity.Web3Api.Models
{
    public class CloudFunctionResult<T>
    {
        [JsonProperty("result")]
        [Preserve]
        public T Result { get; set; }
    }
}