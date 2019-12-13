using Newtonsoft.Json;

namespace FirebasePush.Net.Messages
{
    public class ResponseMessageBody
    {
        [JsonProperty("multicast_id")]
        public long MulticastId { get; set; }

        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("failure")]
        public int Failure { get; set; }

        [JsonProperty("canonical_ids")]
        public int CanonicalIds { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }
}
