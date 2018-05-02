using CommunicationModels.Models;
using System.Threading.Tasks;

namespace VkConnector.Services
{
    public interface IUpdatesListener
    {
        Task StartListening(SubscriptionModel subscriptionModel);

        void StopListening(SubscriptionModel subscriptionModel);
    }
}