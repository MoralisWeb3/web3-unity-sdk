using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MoralisUnity.SolanaApi.Models
{
    public class TokenPrice
    {
        [DataContract]
		public class TokenPrice
		{

		[DataMember(Name = "usdPrice", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "usdPrice")]
        public string UsdPrice { get; set; }

		[DataMember(Name = "exchangeName", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "exchangeName")]
        public string ExchangeName { get; set; }

		[DataMember(Name = "exchangeAddress", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "exchangeAddress")]
        public string ExchangeAddress { get; set; }

		[DataMember(Name = "nativePrice", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "nativePrice")]
        public string NativePrice { get; set; }
		
		
		}
		
    }
}