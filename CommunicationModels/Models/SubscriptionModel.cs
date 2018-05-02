using System;

namespace CommunicationModels.Models
{
    [Serializable]
    public class SubscriptionModel
    {
        /// <summary>
        ///     Url, на который будут приходить уведомления о новых сообщениях
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        ///     Токен, для определения пользователя, подписываемого на уводомления
        /// </summary>
        public string UserToken { get; set; }

        /// <summary>
        ///     Информация для авторизации
        /// </summary>
        public AuthorizedUser User { get; set; }
    }
}
