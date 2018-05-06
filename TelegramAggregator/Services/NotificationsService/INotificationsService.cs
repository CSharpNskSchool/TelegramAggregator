using System.Threading.Tasks;
using CommunicationModels.Models;
using TelegramAggregator.Model.Entities;

namespace TelegramAggregator.Services.NotificationsService
{
    public interface INotificationsService
    {
        Task EnableNotifications(BotUser botUser);

        Task SendNotificationToUser(long chatId, RecievedMessage recievedMessage);
    }
}