using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommunicationModels.Models
{
    /// <summary>
    /// Вложение
    /// </summary>
    public class MessageAttachment
    {
        [JsonConverter(typeof(StringEnumConverter))] 
        public enum Type
        {
            Undifined,
            Link,
            Image,
            Video,
            Audio
        }
        
        /// <summary>
        ///     Тип приложения
        /// </summary>
        [Required(ErrorMessage = "Не указан тип приложения к сообщению")]
        public Type AttachmentType { get; set; }
        
        /// <summary>
        ///     Ссылка на приложение
        /// </summary>
        /// <example>
        ///     Type.Image: example.com/img.png
        ///     Type.Url: example.com
        /// </example>
        public Uri AttachmentUri { get; set; }
        
        /// <summary>
        ///     Описание вложения
        /// </summary>
        public string Caption { get; set; }
    }
}