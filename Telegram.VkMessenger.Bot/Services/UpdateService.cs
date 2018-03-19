using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.VkMessenger.Bot.Services.BotCommands;
using VkNet;

namespace Telegram.VkMessenger.Bot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;
        private readonly Dictionary<String, IBotCommand> _botCommands = new Dictionary<String, IBotCommand>()
        {
            {"/login", new BotCommandLogin()}
        };
        
        public UpdateService(IBotService botService, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _logger = logger;
        }

        public async Task EchoAsync(Update update)
        {
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                if (update.Message.Text.StartsWith("/"))
                {
                    await HandleComands(update);
                }
                else
                {
                    await HandleMessages(update);
                }
                
            }
        }
        
        private async Task HandleComands(Update update)
        {
            var message = update.Message;
            if (message.Type != MessageType.Text)
            {
                return;
            }

            var messageComandAndArgs = message.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var commandName = messageComandAndArgs.FirstOrDefault();
            var commandArgs = messageComandAndArgs.Skip(1);

            _logger.LogInformation($"Получена команда {commandName} с аргументами {commandArgs} из диалога: {message.Chat.Id}");

            if (!_botCommands.ContainsKey(commandName))
            {
                return;
            }
            
            await _botCommands[commandName].Execute(commandArgs, _botService, message);
        }
        
        private async Task HandleMessages(Update update)
        {
            var message = update.Message;
            _logger.LogInformation("Получено сообщение из диалога {0}", message.Chat.Id);

            if (message.Type != MessageType.Text)
            {
                return;
            }

            await _botService.Client.SendTextMessageAsync(message.Chat.Id, message.Text);
        }
    }
}