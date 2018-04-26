using System;
using System.Threading.Tasks;
using TelegramAggregator.Data.Entities;
using VkConnector.Client;
using VkConnector.Model;
using VkConnector.Model.Messages;
using VkConnector.Model.Users;

namespace TelegramAggregator.Services.NotificationsService
{
    public class NotificationsService : INotificationsService
    {
        private const string AppHost = "https://bc2ec974.ngrok.io";
        private const string NotificationsListenerRoute = "api/notifications";
        
        private readonly IBotService _botService;
        
        public NotificationsService(IBotService botService)
        {
            _botService = botService;
        }
        
        public async Task EnableNotifications(BotUser botUser)
        {
            var connector = new VkConnectorClient("http://localhost:5000");

            await connector.SetWebHook(new SubscriptionModel()
            {
                Url = new Uri($"{AppHost}/{NotificationsListenerRoute}/{botUser.TelegramChatId}"),
                User = new AuthorizedUser()
                {
                    AccessToken = botUser.VkAccount.AcessToken
                }
            });
        }

        public async Task SendNotificationToUser(long chatId, RecievedMessage recievedMessage)
        {
            var heading = recievedMessage.IsIncoming 
                ? $"{recievedMessage.Sender.Id} "
                : "Вы ";

            heading += recievedMessage.IsIncoming
                ? string.Empty
                : $"{recievedMessage.Sender.Id} ";
            
            heading += recievedMessage.ChatId != -1
                ? $"в {recievedMessage.ChatId}"
                : string.Empty;
            
            await _botService.Client.SendTextMessageAsync(chatId, $"{heading}:\r\n {recievedMessage.Body.Text}");
        }
    }
}