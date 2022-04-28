using Newtonsoft.Json;

namespace MoralisUnity.SolanaApi.Models
{
    public class CloudFunctionResult<T>
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
