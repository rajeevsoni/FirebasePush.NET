using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FirebasePush.Net.Contracts;
using FirebasePush.Net.Messages;
using Microsoft.Extensions.Logging;

namespace FirebasePush.Net
{
    public class PushService : IPushService
    {
        readonly string _firebaseEndpoint;
        readonly string _firebaseServerKey;
        readonly ILogger<PushService> _logger;
        readonly int _maxRetries;
        readonly int _waitBetweenRetry;

        public PushService(string firebaseServerKey, string firebaseEndPoint, int maxRetries, int waitBetweenRetry, ILogger<PushService> logger)
        {
            _firebaseServerKey = firebaseServerKey;
            _firebaseEndpoint = firebaseEndPoint;
            _maxRetries = maxRetries;
            _waitBetweenRetry = waitBetweenRetry;
            _logger = logger;
        }

        public async Task<PushResponse> Push(PushRequest pushRequest)
        {
            if (pushRequest.Body == null || pushRequest.Body.RegistrationIds.Count == 0)
            {
                throw new ResponseMessageException(System.Enum.GetName(typeof(Error), Error.MissingRegistration), HttpStatusCode.BadRequest);
            }

            int _attempts = 0;

            while (_attempts < _maxRetries)
            {
                try
                {
                    var response = await SendPushNotification(pushRequest);
                    HandleResponse(pushRequest, response);
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    _attempts++;
                    _logger.LogWarning(ex, ex.Message);
                    await Task.Delay(_waitBetweenRetry * _attempts);
                }
                catch (ResponseMessageException ex) when (ex.ResponseCode == HttpStatusCode.ServiceUnavailable || ex.ResponseCode == HttpStatusCode.InternalServerError)
                {
                    _attempts++;
                    _logger.LogWarning(ex, ex.Message);
                    await Task.Delay(_waitBetweenRetry * _attempts);
                }
            }
            _logger.LogError("Push notification failed for below:");
            _logger.LogError(JsonConvert.SerializeObject(pushRequest));
            throw new Exception("Service can't send Push Notification.");
        }

        private async Task<PushResponse> SendPushNotification(PushRequest firebaseRequestMessage)
        {
            var jsonBody = firebaseRequestMessage.BodyToJSON;
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _firebaseEndpoint)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, firebaseRequestMessage.Header.ContentType)
            };
            httpRequest.Headers.TryAddWithoutValidation("Authorization", "key=" + _firebaseServerKey);

            using (var httpClient = new HttpClient())
            using (HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest))
            {
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new ResponseMessageException(httpResponse.ReasonPhrase, httpResponse.StatusCode);
                }

                var responsePayload = await httpResponse.Content.ReadAsStringAsync();
                ResponseMessageBody cloudResponseBody = JsonConvert.DeserializeObject<ResponseMessageBody>(responsePayload);

                return new PushResponse
                {
                    Header = new ResponseMessageHeader() { ResponseStatusCode = httpResponse.StatusCode },
                    Body = cloudResponseBody
                };
            }
        }

        private void HandleResponse(PushRequest request, PushResponse response)
        {
            if (response.Body.Failure == 0)
            {
                _logger.LogError(JsonConvert.SerializeObject(response));
            }
            else
            {
                for (var i = 0; i < response.Body.Results.Length; i++)
                {
                    var result = response.Body.Results[i];
                    if (result.Error == Error.InvalidRegistration || result.Error == Error.NotRegistered)
                    {
                        _logger.LogError($"{result.Error.ToString()}: error occured for FCMToken: {request.Body.RegistrationIds[i]}");
                    }
                }
            }
        }
    }
}
