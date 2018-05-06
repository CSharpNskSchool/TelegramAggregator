using System;
using System.Threading.Tasks;
using CommunicationModels.Models;
using Telegram.Bot;
using Telegram.Bot.Framework;
using Telegram.Bot.Types.Enums;
using TelegramAggregator.Model.Entities;
using TelegramAggregator.Model.Extensions;
using VkConnector.Client;
using VkNet;

namespace TelegramAggregator.Services.NotificationsService
{
    public class NotificationsService : INotificationsService
    {
        private const string AppHost = "http://localhost:8443";
        private const string ConnectorHost = "http://localhost:5000";
        private const string NotificationsListenerRoute = "api/notifications";
        private readonly AggregatorBot _bot;

        public NotificationsService(AggregatorBot bot)
        {
            _bot = bot;
        }

        public async Task EnableNotifications(BotUser botUser)
        {
            if (botUser.VkAccount == null)
            {
                throw new ArgumentNullException(nameof(botUser), "Для включения уведомлеий нужен аккаунт Вконтакте");
            }
            
            var connector = new ConnectorsClient(ConnectorHost);

            await connector.SetWebHook(new SubscriptionModel
            {
                Url = new Uri($"{AppHost}/{NotificationsListenerRoute}/{botUser.TelegramChatId}"),
                User = new AuthorizedUser
                {
                    AccessToken = botUser.VkAccount.AcessToken
                }
            });
        }

        public async Task SendNotificationToUser(long chatId, RecievedMessage recievedMessage)
        {
            var api = new VkApi();
            var senderInfo = api.GetUserById(recievedMessage.Sender.Id);
                
            var heading = recievedMessage.IsIncoming
                ? $"{senderInfo.FirstName} {senderInfo.LastName} "
                : "Вы ";

            heading += recievedMessage.IsIncoming && recievedMessage.ChatId != -1
                ? string.Empty
                : $"{senderInfo.FirstName} {senderInfo.LastName} ";

            heading += recievedMessage.ChatId != -1
                ? $"в беседе c{recievedMessage.ChatId}"
                : string.Empty;

            await _bot.Client.SendTextMessageAsync(chatId, $"`{heading}`\r\n {recievedMessage.Body.Text}", ParseMode.Markdown);
        }
    }
}