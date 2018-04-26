using System.Threading.Tasks;
using VkConnector.Model;

namespace VkConnector.Services
{
    public interface IUpdatesListener
    {
        Task StartListening(SubscriptionModel subscriptionModel);

        void StopListening(SubscriptionModel subscriptionModel);
    }
}