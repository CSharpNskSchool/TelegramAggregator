using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunicationModels.Models;
using Newtonsoft.Json;

namespace VkConnector.Client
{
    public class ConnectorsClient : IConnectorsClient
    {
        private readonly string _apiUrl;
        private readonly string _apiVersion;
        private readonly HttpClient _httpClient;

        public ConnectorsClient(string apiUrl, string apiVersion = "v0")
        {
            _apiUrl = apiUrl;
            _apiVersion = apiVersion;
            _httpClient = new HttpClient();
        }

        public Task SendMessage(TransmittedMessage transmittedMessage)
        {
            return InvokeApiMethod(HttpMethod.Post, "transfer", content: transmittedMessage);
        }

        public Task SetWebHook(SubscriptionModel subscriptionModel)
        {
            return InvokeApiMethod(HttpMethod.Post, "subscribe", content: subscriptionModel);
        }

        private Task InvokeApiMethod(
            HttpMethod httpMethod,
            string path,
            string ticket = null,
            object content = null,
            Dictionary<string, string> headers = null)
        {
            return MakeRequest(httpMethod, path, ticket, content, headers);
        }

        private async Task<T> InvokeApiMethod<T>(HttpMethod httpMethod,
            string path,
            string ticket = null,
            object content = null,
            Dictionary<string, string> headers = null)
        {
            var response = await MakeRequest(httpMethod, path, ticket, content, headers);
            var serializedObject = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        private async Task<HttpResponseMessage> MakeRequest(HttpMethod httpMethod,
            string path,
            string bearerToken = null,
            object content = null,
            Dictionary<string, string> headers = null)
        {
            // var request = new HttpRequestMessage(httpMethod, $"{_apiUrl}/api/{_apiVersion}/{path}");
            var request = new HttpRequestMessage(httpMethod, $"{_apiUrl}/api/{path}");

            if (bearerToken != null)
            {
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
            }

            if (content != null)
            {
                var serializedContent = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                HandleError(response);
            }

            return response;
        }

        private void HandleError(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }
    }
}