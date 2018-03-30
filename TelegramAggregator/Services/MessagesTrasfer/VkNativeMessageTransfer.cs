using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAggregator.Data.Entities;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace TelegramAggregator.Services.MessagesTrasfer
{
    /// <summary>
    /// Кто читает, обрати внимание:
    /// В этом классе много работы с vk-api, которой по идее тут быть не должно.
    /// Код оставил для того, кто будет писать VkConnector, т.к. тут уже выполнена часть его работы
    /// И для того, кто будет писать MessageTransferApi, чтобы было представление,
    /// с какими типами сообщений надо уметь работать
    /// </summary>
    public class VkNativeMessageTransfer : IMessageTransfer
    {
        private readonly IBotService _botService;
        
        public VkNativeMessageTransfer(IBotService botService)
        {
            _botService = botService;
        }
        
        public async Task Transfer(BotUser botUser, Message message)
        {
            var api = new VkApi();

            if (botUser.VkAccount == null)
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, 
                    $"Для отправки сообщений привяжите аккаунт Вконтакте");
                return;
            }
            
            await api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = botUser.VkAccount.AcessToken
            });

            switch (message.Type)
            {
                case MessageType.Text:
                    await TransferTextMessage(api, botUser, message);
                    break;
                case MessageType.Photo:
                    await TransferPhotoMessage(api, botUser, message);
                    break;
                case MessageType.Sticker:
                    await TransferStickerMessage(api, botUser, message);
                    break;
// TODO: Audio, Video, Voice, Document, Location, Contact.
                default:
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, 
                        $"Отправка сообщений этого типа еще не реализована");
                    break;
            }
        }
        
        private async Task TransferTextMessage(VkApi api, BotUser botUser, Message message)
        {
            await api.Messages.SendAsync(new MessagesSendParams
            {
                Message = message.Text,
                PeerId = botUser.VkAccount.CurrentPeer
            });
        }
        
        private async Task TransferStickerMessage(VkApi api, BotUser botUser, Message message)
        {
            var stickerFile = await _botService.Client.GetFileAsync(message.Sticker.FileId);
            var sticker = UploadPhotoToVk(stickerFile, api);

            await api.Messages.SendAsync(new MessagesSendParams
            {
                Message = message.Caption,
                PeerId = botUser.VkAccount.CurrentPeer,
                Attachments = new[]
                {
                    sticker
                }
            });
        }

        private async Task TransferPhotoMessage(VkApi api, BotUser botUser, Message message)
        {
            var photoSize = message.Photo
                .OrderBy(size => size.FileSize)
                .FirstOrDefault();

            if (photoSize == null)
            {
                throw new ArgumentNullException();
            }

            var photoFile = await _botService.Client.GetFileAsync(photoSize.FileId);
            var photo = UploadPhotoToVk(photoFile, api);

            await api.Messages.SendAsync(new MessagesSendParams
            {
                Message = message.Caption,
                PeerId = botUser.VkAccount.CurrentPeer,
                Attachments = new[]
                {
                    photo
                }
            });
        }

        private Photo UploadPhotoToVk(File photoFile, VkApi api)
        {
            var wc = new WebClient();
            wc.DownloadFile(
                $"https://api.telegram.org/file/bot{_botService.Configuration.BotToken}/{photoFile.FilePath}",
                "devnull.png");

            var uploadServer = api.Photo.GetMessagesUploadServer(1);
            var responce = wc.UploadFile(uploadServer.UploadUrl, "devnull.png");
            var responseImg = Encoding.ASCII.GetString(responce);

            return api.Photo.SaveMessagesPhoto(responseImg).First();
        }

    }
}