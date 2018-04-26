using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;
using TelegramAggregator.Extentions;
using TelegramAggregator.Services.NotificationsService;

namespace TelegramAggregator.Services.BotCommands
{
    public class BotCommandLogin : IBotCommand
    {
        public async Task Execute(IEnumerable<string> commandArgs, IBotService botService, INotificationsService notificationsService,
            BotUser botUser,
            Message message)
        {
            if (commandArgs.Count() != 1)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, "Формат команды: /login <acess_token>");
                return;
            }

            var vkAcessToken = commandArgs.First();

            try
            {
                var vkUser = botUser.AuthorizeVk(vkAcessToken);
                notificationsService.EnableNotifications(botUser);
                
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Вы авторизированы как {vkUser.FirstName} {vkUser.LastName}");
            }
            catch (Exception e)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, e.Message);
            }
        }
    }
}