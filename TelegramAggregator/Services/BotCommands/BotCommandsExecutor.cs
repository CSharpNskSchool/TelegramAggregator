using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.BotCommands
{
    public static class BotCommandsExecutor
    {
        private static readonly Dictionary<string, IBotCommand> BotCommands = new Dictionary<string, IBotCommand>
        {
            {"/login", new BotCommandLogin()},
            {"/setPeer", new BotCommandSetPeer()},
            {"/whoami", new BotCommandWhoAmI()}
        };

        public static async Task HandleComands(IBotService botService, BotUser botUser, Message message)
        {
            var messageComandAndArgs = message.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var commandName = messageComandAndArgs.FirstOrDefault();
            var commandArgs = messageComandAndArgs.Skip(1);

            if (!BotCommands.ContainsKey(commandName))
            {
                await botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"Ошибка выполнения команды {commandName}: неизвестная команда");
                return;
            }

            await BotCommands[commandName].Execute(commandArgs, botService, botUser, message);
        }
    }
}