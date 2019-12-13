using FirebasePush.Net.Serialization;
using Newtonsoft.Json;

namespace FirebasePush.Net.Messages
{
    public class PushRequest
    {
        readonly JsonSerializerSettings _jsonSerializerSettings;

        public PushRequest()
        {
            Header = new RequestMessageHeader();
            Body = new RequestMessageBody();

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new PropertyNameResolver()
            };
        }
        public RequestMessageHeader Header { get; set; }

        public RequestMessageBody Body { get; set; }
        public string BodyToJSON
        {
            get { return JsonConvert.SerializeObject(Body, Formatting.Indented, _jsonSerializerSettings); }
        }        
    }
}
