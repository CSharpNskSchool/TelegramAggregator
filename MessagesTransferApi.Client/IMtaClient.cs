using CommunicationModels.Models;
using MessagesTransferApi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessagesTransferApi.Client
{
    public interface IMtaClient
    {
        Task<List<Connector>> GetConnectors();
        Task<List<User>> GetUsers();
        Task<List<string>> GetNetworks();
        Task<List<NetworkAuthData>> GetAccounts();

        Task<string> AddUser(UserData userData);
        Task<string> AttachAccount(Account account, string userToken);
        Task<string> AddConnector(ConnectorData connectorData);
        Task<string> SendMessageToConnector(AggregatorMessage message, string userToken);
        Task<string> SendMessageToAggregator(int userID, RecievedMessage message, string networkName = "VK");
    }
}
