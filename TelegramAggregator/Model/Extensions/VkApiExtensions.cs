using System;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace TelegramAggregator.Model.Extensions
{
    public static class VkApiExtensions
    {
        public static User GetUserById(this VkApi vkApi, long id)
        {
            var screenName = $"id{id}";
            
            var peers = vkApi.Users.Get(new[] {screenName}, ProfileFields.All, null, true);

            if (!peers.Any())
            {
                throw new ArgumentException($"Пользователей Вконтакте с именем {screenName} не найдено");
            }

            return peers.First();
        }
    }
}