using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.VkMessenger.Bot.Models;

namespace Telegram.VkMessenger.Bot.Services.BotCommands
{
    public static class BotCommandsExecutor
    {
        private static readonly Dictionary<string, IBotCommand> BotCommands = new Dictionary<string, IBotCommand>
        {
            {"/login", new BotCommandLogin()},
            {"/whoami", new BotCommandWhoAmI()}
        };

        public static async Task HandleComands(IBotService botService, UserContext userContext, Message message)
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

            await BotCommands[commandName].Execute(commandArgs, botService, userContext, message);
        }
    }
}