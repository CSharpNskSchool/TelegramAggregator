using CommunicationModels.Models;
using System.Threading.Tasks;

namespace VkConnector.Services
{
    public interface IUpdatesListener
    {
        Task StartListening(SubscriptionModel subscriptionModel);

        bool StopListening(SubscriptionModel subscriptionModel);
    }
}