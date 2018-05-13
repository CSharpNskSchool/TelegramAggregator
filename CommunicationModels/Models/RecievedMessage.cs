namespace CommunicationModels.Models
{
    /// <summary>
    ///     Получаемое пользователем сообщение
    /// </summary>
    public class RecievedMessage
    {
        public RecievedMessage(long chatId, ExternalUser sender, bool isIncoming, Message message, bool isForwarded)
        {
            ChatId = chatId;
            Sender = sender;
            IsIncoming = isIncoming;
            Message = message;
            IsForwarded = isForwarded;
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
        ///     true - пересланное, false - обычное
        /// </summary>
        public bool IsForwarded { get; }

        /// <summary>
        ///     Тело сообщения
        /// </summary>
        public Message Message { get; }
    }
}