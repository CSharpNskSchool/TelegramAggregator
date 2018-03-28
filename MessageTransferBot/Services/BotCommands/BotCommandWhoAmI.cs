using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageTransferBot.Data.Entities;
using MessageTransferBot.Extentions;
using Telegram.Bot.Types;

namespace MessageTransferBot.Services.BotCommands
{
    public class BotCommandWhoAmI : IBotCommand
    {
        public async Task Execute(IEnumerable<string> commandArgs, IBotService botService, BotUser botUser,
            Message message)
        {
            if (botUser.VkAccount == null)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Вы не авторизированы");
            }

            try
            {
                var vkUser = botUser.GetProfileInfoVk();

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