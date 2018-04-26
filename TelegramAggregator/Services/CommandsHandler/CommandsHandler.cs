using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;
using TelegramAggregator.Services.BotCommands;
using TelegramAggregator.Services.NotificationsService;

namespace TelegramAggregator.Services.CommandsHandler
{
    public class CommandsHandler : ICommandsHandler
    {
        private readonly IBotService _botService;
        private readonly INotificationsService _notificationsService;
        private static readonly Dictionary<string, IBotCommand> BotCommands = new Dictionary<string, IBotCommand>
        {
            {"/login", new BotCommandLogin()},
            {"/setPeer", new BotCommandSetPeer()},
            {"/whoami", new BotCommandWhoAmI()}
        };
        
        public CommandsHandler(IBotService botService, INotificationsService notificationsService)
        {
            _botService = botService;
            _notificationsService = notificationsService;
        }

        public async Task Transfer(BotUser botUser, Message commandMessage)
        {
            var messageComandAndArgs = commandMessage.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var commandName = messageComandAndArgs.FirstOrDefault();
            var commandArgs = messageComandAndArgs.Skip(1);

            if (!BotCommands.ContainsKey(commandName))
            {
                await _botService.Client.SendTextMessageAsync(commandMessage.Chat.Id,
                    $"Ошибка выполнения команды {commandName}: неизвестная команда");
                return;
            }

            await BotCommands[commandName].Execute(commandArgs, _botService, _notificationsService, botUser, commandMessage);
        }
    }
}