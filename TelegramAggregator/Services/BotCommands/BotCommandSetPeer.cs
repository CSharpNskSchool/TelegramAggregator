using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;
using TelegramAggregator.Extentions;

namespace TelegramAggregator.Services.BotCommands
{
    public class BotCommandSetPeer : IBotCommand
    {
        public async Task Execute(IEnumerable<string> commandArgs, IBotService botService, BotUser botUser,
            Message message)
        {
            if (commandArgs.Count() != 1)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, "Формат команды: /setPeer <screenName>");
                return;
            }

            if (botUser.VkAccount == null)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    "Для отправки сообщений привяжите аккаунт Вконтакте");
                return;
            }

            var peerScreename = commandArgs.First();

            try
            {
                var peer = botUser.SetPeer(peerScreename);
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Вы пишите: {peer.FirstName} {peer.LastName}");
            }
            catch (Exception e)
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id, e.Message);
            }
        }
    }
}