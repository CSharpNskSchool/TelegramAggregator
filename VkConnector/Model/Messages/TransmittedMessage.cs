using System.ComponentModel.DataAnnotations;
using VkConnector.Model.Users;

namespace VkConnector.Model.Messages
{
    /// <summary>
    ///     Передаваемое от пользователя соц. сети сообщение
    /// </summary>
    public class TransmittedMessage
    {
        /// <summary>
        ///     Авторизованный отправитель сообщения
        /// </summary>
        [Required(ErrorMessage = "Не передана информации для аутентификации")]
        public AuthorizedUser AuthorizedSender { get; set; }

        /// <summary>
        ///     Получатели сообщения
        /// </summary>
        [Required(ErrorMessage = "Не указан получатель")]
        public ExternalUser Receiver { get; set; }


        /// <summary>
        ///     Тело сообщения
        /// </summary>
        [Required(ErrorMessage = "Нету тела сообщения")]
        public MessageBody Body { get; set; }
    }
}