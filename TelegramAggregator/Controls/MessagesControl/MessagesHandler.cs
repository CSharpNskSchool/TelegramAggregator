using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationModels.Models;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAggregator.Model.Repositories;
using VkConnector.Client;
using Message = CommunicationModels.Models.Message;

namespace TelegramAggregator.Controls.MessagesControl
{
    public class MessagesHandler : IUpdateHandler
    {
        private readonly IBotUserRepository _botUserRepository;
        private readonly AggregatorBotConfiguration _aggregatorBotConfiguration;

        public MessagesHandler(IBotUserRepository botUserRepository, AggregatorBotConfiguration aggregatorBotConfiguration)
        {
            _botUserRepository = botUserRepository;
            _aggregatorBotConfiguration = aggregatorBotConfiguration;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            return update.Type == UpdateType.Message;
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var message = update.Message;
            var botUser = _botUserRepository.GetByTelegramId(message.Chat.Id);
            var connector = new ConnectorsClient("http://localhost:5000");

            await connector.SendMessage(new TransmittedMessage
            {
                AuthorizedSender = new AuthorizedUser
                {
                    AccessToken = botUser.VkAccount.AcessToken
                },
                Message = new Message
                {
                    Body = new MessageBody(message.Text),
                    Attachments = AssemblyMessageAttachments(bot, message),
                    Receiver = new ExternalUser(botUser.VkAccount.CurrentPeer)
                }
            });

            // TODO: Добавить к уведомлению о доставке кнопку "Удалить сообщение" и "Редактировать"
            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Cообщение доставлено",
                replyToMessageId: message.MessageId);

            return UpdateHandlingResult.Handled;
        }

        private IEnumerable<MessageAttachment> AssemblyMessageAttachments(IBot bot, Telegram.Bot.Types.Message message)
        {
            switch (message.Type)
            {
                case MessageType.Photo:
                    yield return GetMessagePhoto(bot, message).Result;
                    break;
                case MessageType.Sticker:
                    yield return GetMessageSticker(bot, message).Result;
                    break; 
                default:
                    break;
            }
        }

        private async Task<MessageAttachment> GetMessagePhoto(IBot bot, Telegram.Bot.Types.Message message)
        {
            if (message.Type != MessageType.Photo)
                return null;
            
            var photoSize = message.Photo
                .OrderBy(size => size.FileSize)
                .LastOrDefault();

            var photoFile = await bot.Client.GetFileAsync(photoSize.FileId);

            return new MessageAttachment()
            {
                AttachmentType = MessageAttachment.Type.Image,
                AttachmentUri = new Uri($"https://api.telegram.org/file/bot{bot.Options.ApiToken}/{photoFile.FilePath}")
            };
        }
        
        private async Task<MessageAttachment> GetMessageSticker(IBot bot, Telegram.Bot.Types.Message message)
        {
            if (message.Type != MessageType.Sticker)
                return null;
            
            var stickerFile = await bot.Client.GetFileAsync(message.Sticker.FileId);

            return new MessageAttachment()
            {
                AttachmentType = MessageAttachment.Type.Image,
                AttachmentUri = new Uri($"https://api.telegram.org/file/bot{bot.Options.ApiToken}/{stickerFile.FilePath}")
            };
        }
    }
}