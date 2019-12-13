namespace FirebasePush.Net.Messages
{
    public class RequestMessageHeader
    {
        public RequestMessageHeader(string contentType = "application/json")
        {
            ContentType = contentType;
        }
        public string Authorization { get; set; }
        public string ContentType { get; set; }
    }
}
