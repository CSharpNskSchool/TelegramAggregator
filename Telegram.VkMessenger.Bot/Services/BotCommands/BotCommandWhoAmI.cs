using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.VkMessenger.Bot.Models;
using VkNet;

namespace Telegram.VkMessenger.Bot.Services.BotCommands
{
    public class BotCommandWhoAmI : IBotCommand
    {
        public async Task Execute(IEnumerable<string> commandArgs, IBotService botService,
            UserContext userContext, Message message)
        {
            var user = await userContext.Users.FirstOrDefaultAsync(u => u.TelegramId == message.From.Id);
            
            if (user == null)
            {
                return;
            }
            
            try
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams
                {
                    AccessToken = user.VkAcessToken
                });
                
                var userInfo = await api.Account.GetProfileInfoAsync();
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Вы авторизированы как {userInfo.FirstName} {userInfo.LastName}");
            }
            catch (Exception e)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, $"Ошибка авторизации: {e.Message}");
            }
        }
    }
}