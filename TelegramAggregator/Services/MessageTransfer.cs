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

namespace TelegramAggregator.Services
{
    public static class MessageTransfer
    {
        public static async Task TransferToVk(Message message, BotUser botUser, IBotService _botService)
        {
            try
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

                if (message.Type == MessageType.Text)
                {
                    await TransferTextMessage(api, botUser, message);
                }
                else if (message.Type == MessageType.Photo)
                {
                    await TransferPhotoMessage(api, _botService, botUser, message);
                }
                else if (message.Type == MessageType.Sticker)
                {
                    await TransferStickerMessage(api, _botService, botUser, message);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Ошибка отправки сообщения: {e.Message}");
            }
        }

        private static async Task TransferStickerMessage(VkApi api, IBotService _botService, BotUser botUser,
            Message message)
        {
            var sticker = message.Sticker;
            var stickerFile = await _botService.Client.GetFileAsync(sticker.FileId);

            var photo = UploadPhotoToVk(stickerFile, api, _botService);

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

        private static async Task TransferPhotoMessage(VkApi api, IBotService _botService, BotUser botUser,
            Message message)
        {
            var photoSize = message.Photo
                .OrderBy(size => size.FileSize)
                .FirstOrDefault();

            var photoFile = await _botService.Client.GetFileAsync(photoSize.FileId);

            var photo = UploadPhotoToVk(photoFile, api, _botService);

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

        private static Photo UploadPhotoToVk(File photoFile, VkApi api, IBotService _botService)
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

        private static async Task TransferTextMessage(VkApi api, BotUser botUser, Message message)
        {
            await api.Messages.SendAsync(new MessagesSendParams
            {
                Message = message.Text,
                PeerId = botUser.VkAccount.CurrentPeer
            });
        }
    }
}