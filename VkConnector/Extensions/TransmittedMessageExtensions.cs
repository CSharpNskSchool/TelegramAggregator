using CommunicationModels.Models;
using System.Threading.Tasks;
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

            var messageSendParams = transmittedMessage.GetMessageSendParams(transmittedMessage.Message.Receiver.Id);
            await api.Messages.SendAsync(messageSendParams);
        }

        private static MessagesSendParams GetMessageSendParams(
            this TransmittedMessage transmittedMessage,
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

            // TODO: для Atachments заполняем соответвующие поля

            return result;
        }
    }
}