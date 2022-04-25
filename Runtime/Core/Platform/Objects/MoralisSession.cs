

namespace MoralisUnity.Platform.Objects
{
    public class MoralisSession : MoralisObject
    {
        public MoralisSession() : base("_Session") { }

       	// [JsonProperty("sessionToken")]
        public new string sessionToken { get; set; }
    }
}
