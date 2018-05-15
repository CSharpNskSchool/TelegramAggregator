using CommunicationModels.Models;
using MessagesTransferApi.Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessagesTransferApi.Client
{
    public class MtaClient : IMtaClient
    {
        private readonly string baseUrl;
        private readonly HttpClient httpClient = new HttpClient();

        public MtaClient(string baseUrl, string apiVersion = "v0")
        {
            this.baseUrl = $"{baseUrl}{(baseUrl[baseUrl.Length - 1] == '/' ? "" : "/")}{apiVersion}";
        }

        public async Task<List<string>> GetNetworks()
        {
            var jsonContent = await SendGetRequestAsync($"{baseUrl}/api/aggregator/networks");
            return JsonConvert.DeserializeObject<List<string>>(jsonContent);
        }

        public async Task<List<NetworkAuthData>> GetAccounts()
        {
            var jsonContent = await SendGetRequestAsync($"{baseUrl}/api/aggregator/accounts");
            return JsonConvert.DeserializeObject<List<NetworkAuthData>>(jsonContent);
        }

        public async Task<List<Connector>> GetConnectors()
        {
            var jsonContent = await SendGetRequestAsync($"{baseUrl}/api/connector");
            return JsonConvert.DeserializeObject<List<Connector>>(jsonContent);
        }

        public async Task<List<User>> GetUsers()
        {
            var jsonContent = await SendGetRequestAsync($"{baseUrl}/api/aggregator/users");
            return JsonConvert.DeserializeObject<List<User>>(jsonContent);
        }

        public async Task<string> AddConnector(ConnectorData connectorData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(connectorData), Encoding.UTF8, "application/json");
            var url = $"{baseUrl}/api/connector";

            return await SendPostRequestAsync(url, content);
        }

        public async Task<string> AddUser(UserData userData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json");
            var url = $"{baseUrl}/api/aggregator/users";

            return await SendPostRequestAsync(url, content);
        }

        public async Task<string> AttachAccount(Account account, string userToken)
        {
            var content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
            var url = $"{baseUrl}/api/aggregator/Users/Accounts?userToken={userToken}";

            return await SendPostRequestAsync(url, content);
        }

        public async Task<string> SendMessageToAggregator(int userID, RecievedMessage message, string networkName = "VK")
        {
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var url = $"{baseUrl}/api/connector/Messages/{userID}?networkName={networkName}";

            return await SendPostRequestAsync(url, content);
        }

        public async Task<string> SendMessageToConnector(AggregatorMessage message, string userToken)
        {
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var url = $"{baseUrl}/api/aggregator/Messages?userToken={userToken}";

            return await SendPostRequestAsync(url, content);
        }

        private async Task<string> SendGetRequestAsync(string url)
        {
            var answer = await new HttpClient().GetAsync(url);
            return await answer.Content.ReadAsStringAsync();
        }

        private async Task<string> SendPostRequestAsync(string url, StringContent content)
        {
            var answer = await new HttpClient().PostAsync(url, content);
            return await answer.Content.ReadAsStringAsync();
        }
    }
}
