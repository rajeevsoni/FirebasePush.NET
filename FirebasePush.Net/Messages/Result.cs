using Newtonsoft.Json;

namespace FirebasePush.Net.Messages
{
    public class Result
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("registration_id")]
        public string RegistrationId { get; set; }

        [JsonProperty("error")]
        public Error? Error { get; set; }
    }
}
