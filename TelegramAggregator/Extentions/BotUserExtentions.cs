using System;
using System.Linq;
using TelegramAggregator.Data.Entities;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace TelegramAggregator.Extentions
{
    public static class BotUserExtentions
    {
        public static User AuthorizeVk(this BotUser botUser, string acessToken)
        {
            if (botUser == null)
            {
                throw new ArgumentNullException();
            }

            var api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                AccessToken = acessToken
            });

            var screenName = api.Account.GetProfileInfo().ScreenName;
            var user = GetUserByScreenName(api, screenName);

            botUser.VkAccount = new VkAccount
            {
                Id = user.Id,
                AcessToken = acessToken,
                CurrentPeer = user.Id
            };

            return user;
        }

        public static User GetProfileInfoVk(this BotUser botUser)
        {
            if (botUser?.VkAccount == null)
            {
                throw new ArgumentNullException();
            }

            var api = GetAuthorizedVkApi(botUser);
            var screenName = api.Account.GetProfileInfo().ScreenName;
            var user = GetUserByScreenName(api, screenName);

            return user;
        }

        public static User SetPeer(this BotUser botUser, string screenName)
        {
            if (botUser?.VkAccount == null)
            {
                throw new ArgumentNullException();
            }

            var api = GetAuthorizedVkApi(botUser);
            var peer = GetUserByScreenName(api, screenName);

            botUser.VkAccount.CurrentPeer = peer.Id;
            return peer;
        }

        private static VkApi GetAuthorizedVkApi(BotUser botUser)
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                AccessToken = botUser.VkAccount.AcessToken
            });
            return api;
        }

        private static User GetUserByScreenName(VkApi api, string screenName)
        {
            var peers = api.Users.Get(new[] {screenName}, ProfileFields.All, null, true);

            if (!peers.Any())
            {
                throw new ArgumentException($"Пользователей Вконтакте с именем {screenName} не найдено");
            }

            var peer = peers.First();
            return peer;
        }
    }
}