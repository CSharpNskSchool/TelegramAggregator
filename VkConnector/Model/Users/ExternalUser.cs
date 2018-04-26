namespace VkConnector.Model.Users
{
    /// <summary>
    ///     Пользователь соц.сети, с которым пользователь бота хочет взаимодействовать.
    /// </summary>
    public class ExternalUser
    {
        public ExternalUser(long id)
        {
            Id = id;
        }

        /// <summary>
        ///     Id этого челика в соц.сети
        /// </summary>
        public long Id { get; }
    }
}