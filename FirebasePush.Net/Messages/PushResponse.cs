using FirebasePush.Net.Messages;

namespace FirebasePush.Net
{
    public class PushResponse
    {
        public ResponseMessageHeader Header { get; set; }

        public ResponseMessageBody Body { get; set; }
    }
   
}
