using CommunicationModels.Models;
using System.Threading.Tasks;

namespace VkConnector.Client
{
    public interface IConnectorsClient
    {
        Task SendMessage(TransmittedMessage transmittedMessage);
        Task SetWebHook(SubscriptionModel subscriptionModel);
    }
}