using System.Threading.Tasks;
using TelegramAggregator.Data.Entities;
using VkConnector.Model.Messages;

namespace TelegramAggregator.Services.NotificationsService
{
    public interface INotificationsService
    {
        Task EnableNotifications(BotUser botUser);

        Task SendNotificationToUser(long chatId, RecievedMessage recievedMessage);
    }
}