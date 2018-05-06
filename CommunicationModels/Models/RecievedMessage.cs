namespace CommunicationModels.Models
{
    /// <summary>
    ///     Получаемое пользователем сообщение
    /// </summary>
    public class RecievedMessage
    {
        public RecievedMessage(long chatId, ExternalUser sender, bool isIncoming, MessageBody body)
        {
            ChatId = chatId;
            Sender = sender;
            IsIncoming = isIncoming;
            Body = body;
        }

        /// <summary>
        ///     Отправитель (челик из соц.сети).
        /// </summary>
        public ExternalUser Sender { get; }

        /// <summary>
        ///     Идентификатор беседы. Для личных переписок -1
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        ///     true - полученное, false - отправленное
        /// </summary>
        /// <remarks>
        ///     Название класса RecievedMessage намекает на то,
        ///     что тут могут быть только полученные сообщения,
        ///     но по факту мы можем получить уведомление об отправленном
        ///     собой же сообщении. Поэтому и нужно это поле.
        /// </remarks>
        public bool IsIncoming { get; }

        /// <summary>
        ///     Тело сообщения
        /// </summary>
        public MessageBody Body { get; }
    }
}