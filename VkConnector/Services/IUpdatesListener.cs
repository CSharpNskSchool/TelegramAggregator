using System.Threading.Tasks;
using CommunicationModels.Models;

namespace VkConnector.Services
{
    public interface IUpdatesListener
    {
        Task StartListening(SubscriptionModel subscriptionModel);

        void StopListening(SubscriptionModel subscriptionModel);
    }
}