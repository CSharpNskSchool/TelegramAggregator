using System.Threading.Tasks;
using VkNet;
using VkNet.Exception;

namespace VkConnector.Extensions
{
    public static class VkApiExtensions
    {
        /// <summary>
        ///     Авторизация по acess_token с проверкой его валидности
        /// </summary>
        /// <remarks>
        ///     Т.к. поле vkApi.IsAuthorized при авторизации с невалидным acess_token
        ///     остается истиным, а метод api.Authorize не кидает исключение, то пришлось
        ///     написать такую проверку авторизации
        /// </remarks>
        public static async Task CheckedAuthorizeAsync(this VkApi api, string accessToken)
        {
            await api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = accessToken
            });

            try
            {
                await api.Account.SetOfflineAsync();
            }
            catch (UserAuthorizationFailException e)
            {
                throw new UserAuthorizationFailException(
                    $"Ошибка авторизации. Не действительный access_token: {accessToken}");
            }
        }
    }
}