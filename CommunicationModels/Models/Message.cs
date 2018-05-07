using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunicationModels.Models
{
    /// <summary>
    ///     Сообщение для отправки
    /// </summary>
    public class Message
    {
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
        
        /// <summary>
        ///     Приложения к сообщению
        /// </summary>
        public IEnumerable<MessageAttachment> Attachments { get; set; }
    }
}