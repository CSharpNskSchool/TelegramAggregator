using System.Threading.Tasks;
using VkConnector.Model;
using VkConnector.Model.Messages;

namespace VkConnector.Client
{
    public interface IVkConnectorClient
    {
        Task SendMessage(TransmittedMessage transmittedMessage);
        Task SetWebHook(SubscriptionModel subscriptionModel);
    }
}