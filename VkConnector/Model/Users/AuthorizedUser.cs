using System;
using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model.Users
{
    /// <summary>
    ///     Информация, необходимая для аутентификации
    /// </summary>
    [Serializable]
    public class AuthorizedUser
    {
        /// <summary>
        ///     access_token социальной сети
        /// </summary>
        [Required]
        // TODO: добавить в атрибут проверку на корректный ацесс-токен
        public string AccessToken { get; set; }
    }
}