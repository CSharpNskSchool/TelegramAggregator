using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.VkMessenger.Bot.Models;
using VkNet;
using User = Telegram.VkMessenger.Bot.Models.User;

namespace Telegram.VkMessenger.Bot.Services.BotCommands
{
    public class BotCommandLogin : IBotCommand
    {
        public async Task Execute(IEnumerable<string> commandArgs, IBotService botService,
            UserContext userContext, Message message)
        {
            if (commandArgs.Count() != 1)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, $"Формат команды: /login <acess_token>");
                return;
            }

            var vkAcessToken = commandArgs.First();

            // TODO: при успешной авторизации записать указанный токен в бд
            try
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams
                {
                    AccessToken = vkAcessToken
                });
                
                var userInfo = await api.Account.GetProfileInfoAsync();
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Вы будете авторизированы как {userInfo.FirstName} {userInfo.LastName}");

                await userContext.Users.AddAsync(new User
                {
                    TelegramId = message.From.Id,
                    VkAcessToken = vkAcessToken,
                    ActiveVkDialogId = 185014513
                });
                
                await userContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, $"Ошибка авторизации: {e.Message}");
            }
        }
    }
}