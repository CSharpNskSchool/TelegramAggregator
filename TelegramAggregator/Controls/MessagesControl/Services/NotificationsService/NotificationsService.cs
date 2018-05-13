using System;
using System.Threading.Tasks;
using CommunicationModels.Models;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramAggregator.Model.Entities;
using TelegramAggregator.Model.Extensions;
using VkConnector.Client;
using VkNet;

namespace TelegramAggregator.Controls.MessagesControl.Services.NotificationsService
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
                
            var heading = (recievedMessage.IsIncoming
                              ? $"{senderInfo.FirstName} {senderInfo.LastName} "
                              : $"Вы к {senderInfo.FirstName} {senderInfo.LastName} ") + 
                          (recievedMessage.ChatId != -1
                              ? $"в беседе c{recievedMessage.ChatId}"
                              : string.Empty);
            
            var replyMarkup = new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("💬"),
                InlineKeyboardButton.WithCallbackData("➡️"),
                // TODO: для ответа надо кидать ForceReplyMarkup. Красиво и удобно
                InlineKeyboardButton.WithCallbackData("✍️"),
                InlineKeyboardButton.WithCallbackData("❌")
                // TODO: кнопку удалить я бы убрал и попробовал обрабатывать
                // удаленные телеграммовские сообщения так будет проще в использовании
            });

            if (recievedMessage.IsForwarded)
            {
                heading = $"Перасланное сообщение от {senderInfo.FirstName} {senderInfo.LastName} ";
                replyMarkup = null;
            }

            await _bot.Client.SendTextMessageAsync(
                chatId, 
                $"`{heading}`\r\n {recievedMessage.Message.Body.Text}", 
                ParseMode.Markdown,
                replyMarkup: replyMarkup
            );

            foreach (var attachment in recievedMessage.Message.Attachments)
            {
                switch (attachment.AttachmentType)
                {
                    case MessageAttachment.Type.Link:
                    case MessageAttachment.Type.Image:
                    case MessageAttachment.Type.Video:
                    case MessageAttachment.Type.Audio:
                        await _bot.Client.SendTextMessageAsync(chatId,
                            $"`Ссылка на вложение:` {attachment.AttachmentUri}");
                        break;
                    default:
                        await _bot.Client.SendTextMessageAsync(chatId, $"`Необработанное вложение`", ParseMode.Markdown);
                        break;
                }
            }
        }
    }
}