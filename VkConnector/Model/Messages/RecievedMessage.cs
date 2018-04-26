using VkConnector.Model.Users;

namespace VkConnector.Model.Messages
{
    /// <summary>
    ///     Получаемое пользователем сообщение
    /// </summary>
    public class RecievedMessage
    {
        public RecievedMessage(ExternalUser sender, MessageBody body)
        {
            Sender = sender;
            Body = body;
        }

        /// <summary>
        ///     Отправитель (челик из соц.сети)
        /// </summary>
        public ExternalUser Sender { get; }

        /// <summary>
        ///     Тело сообщения
        /// </summary>
        public MessageBody Body { get; }
    }
}