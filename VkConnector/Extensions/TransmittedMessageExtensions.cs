using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunicationModels.Models;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VkConnector.Extensions
{
    public static class TransmittedMessageExtensions
    {
        public static async Task Transfer(this TransmittedMessage transmittedMessage)
        {
            var api = new VkApi();
            await api.CheckedAuthorizeAsync(transmittedMessage.AuthorizedSender.AccessToken);

            var messageSendParams = transmittedMessage.GetMessageSendParams(api, transmittedMessage.Message.Receiver.Id);
            await api.Messages.SendAsync(messageSendParams);
        }

        private static MessagesSendParams GetMessageSendParams(
            this TransmittedMessage transmittedMessage,
            VkApi vkApi,
            long receiver)
        {
            var result = new MessagesSendParams
            {
                PeerId = receiver
            };

            if (transmittedMessage.Message.Body.Text != null)
            {
                result.Message = transmittedMessage.Message.Body.Text;
            }

            result.Attachments = AssemblyMediaAttachments(transmittedMessage, vkApi, result);;
            
            return result;
        }

        private static List<MediaAttachment> AssemblyMediaAttachments(TransmittedMessage transmittedMessage, VkApi vkApi,
            MessagesSendParams result)
        {
            var attachments = new List<MediaAttachment>();
            foreach (var attachment in transmittedMessage.Message.Attachments)
            {
                switch (attachment.AttachmentType)
                {
                    case MessageAttachment.Type.Image:
                        attachments.Add(UploadPhotoToVk(vkApi, attachment.AttachmentUri));
                        break;
                    case MessageAttachment.Type.Video:
                    case MessageAttachment.Type.Audio:
                    case MessageAttachment.Type.Link:
                    default:
                        // Не использую Ling(), т.к. вк-апи почемуто игнорирует ссылки. Поэтому такой костыль
                        result.Message += $"\r\n{attachment.AttachmentType}: {attachment.AttachmentUri}";
                        break;
                }
            }

            return attachments;
        }

        private static Photo UploadPhotoToVk(VkApi api, Uri photoUri)
        {
            var wc = new WebClient();
            wc.DownloadFile(photoUri, "devnull.png");
            var uploadServer = api.Photo.GetMessagesUploadServer(1);
            var responce = wc.UploadFile(uploadServer.UploadUrl, "devnull.png");
            var responseImg = Encoding.ASCII.GetString(responce);

            return api.Photo.SaveMessagesPhoto(responseImg).First();
        }
    }
}