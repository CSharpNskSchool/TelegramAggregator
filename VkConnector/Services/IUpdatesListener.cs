using System.Threading.Tasks;
using CommunicationModels.Models;

namespace VkConnector.Services
{
    public interface IUpdatesListener
    {
        Task StartListening(SubscriptionModel subscriptionModel);

        bool StopListening(SubscriptionModel subscriptionModel);
    }
}