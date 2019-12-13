using System;
using System.Net;

namespace FirebasePush.Net.Messages
{
    public class ResponseMessageException : Exception
    {
        public HttpStatusCode ResponseCode { get; }
       
        public ResponseMessageException()
        {
        }

        public ResponseMessageException(string message) : base(message)
        {
        }

        public ResponseMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ResponseMessageException(string message, HttpStatusCode statusCode) : base(message) 
        { 
            ResponseCode = statusCode; 
        }

    }
}
