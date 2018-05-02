using CommunicationModels.Models;
using System.Threading.Tasks;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.NotificationsService
{
    public interface INotificationsService
    {
        Task EnableNotifications(BotUser botUser);

        Task SendNotificationToUser(long chatId, RecievedMessage recievedMessage);
    }
}