using FirebasePush.Net.Model;
using System.Collections.Generic;

namespace FirebasePush.Net.Messages
{
    public class RequestMessageBody
    {
        public IDictionary<string, string> Data { get; set; }
        public List<string> RegistrationIds { get; set; } = new List<string>();
        public NotificationModel Notification { get; set; }
    }
}
