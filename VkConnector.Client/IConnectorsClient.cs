using System.Threading.Tasks;
using CommunicationModels.Models;

namespace VkConnector.Client
{
    public interface IConnectorsClient
    {
        Task SendMessage(TransmittedMessage transmittedMessage);
        Task SetWebHook(SubscriptionModel subscriptionModel);
    }
}