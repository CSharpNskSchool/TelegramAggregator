using System;
using System.Threading.Tasks;
using VkConnector.Model.Messages;
using VkNet;
using VkNet.Model.RequestParams;

namespace VkConnector.Extensions
{
    public static class TransmittedMessageExtensions
    {
        public static async Task Transfer(this TransmittedMessage transmittedMessage)
        {
            var api = new VkApi();
            await api.CheckedAuthorizeAsync(transmittedMessage.AuthorizedSender.AccessToken);

            var messageSendParams = transmittedMessage.GetMessageSendParams(transmittedMessage.Receiver.Id);
            await api.Messages.SendAsync(messageSendParams);
        }

        private static MessagesSendParams GetMessageSendParams(this TransmittedMessage transmittedMessage,
            long receiver)
        {
            var result = new MessagesSendParams
            {
                PeerId = receiver
            };

            if (transmittedMessage.Body.Text != null)
            {
                result.Message = transmittedMessage.Body.Text;
            }

            // TODO: для Atachments заполняем соответвующие поля

            return result;
        }
    }
}