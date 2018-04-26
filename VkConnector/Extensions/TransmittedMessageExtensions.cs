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

            var resolvedReceiverId = await api.GetResolvedIdAsync(transmittedMessage.Receiver.Id);
            var messageSendParams = transmittedMessage.GetMessageSendParams(resolvedReceiverId);
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

        /// <summary>
        ///     Получение id адресата Вконтакте по короткому имени.
        /// </summary>
        /// <remarks> Отдельно обрабатываются групповые беседы, имеющие идентификаторы вида 'c*номер*' </remarks>
        /// <returns>Id пользователя, если он найден. Иначе null.</returns>
        private static async Task<long> GetResolvedIdAsync(this VkApi api, string receiverScreenName)
        {
            const long groupChatsStartId = 2000000000;

            if (receiverScreenName.StartsWith("c") &&
                long.TryParse(receiverScreenName.Substring(1), out var groupChatId))
            {
                return groupChatsStartId + groupChatId;
            }

            try
            {
                var resolvedReceiver = await api.Utils.ResolveScreenNameAsync(receiverScreenName);
                if (resolvedReceiver.Id != null)
                {
                    return resolvedReceiver.Id.Value;
                }
            }
            catch (Exception e)
            {
                // ignore
            }

            throw new Exception($"Получатель не найден: {receiverScreenName}");
        }
    }
}