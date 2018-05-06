using System.ComponentModel.DataAnnotations;

namespace CommunicationModels.Models
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
        ///     Содержимое сообщения
        /// </summary>
        [Required(ErrorMessage = "Сообщение должно быть определено")]
        public Message Message { get; set; }
    }
}