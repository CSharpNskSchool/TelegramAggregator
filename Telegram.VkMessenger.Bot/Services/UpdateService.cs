using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.VkMessenger.Bot.Models;
using Telegram.VkMessenger.Bot.Services.BotCommands;

namespace Telegram.VkMessenger.Bot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly UserContext _userContext;
        private readonly ILogger<UpdateService> _logger;
        private readonly Dictionary<String, IBotCommand> _botCommands = new Dictionary<String, IBotCommand>()
        {
            {"/login", new BotCommandLogin()},
            {"/whoami", new BotCommandWhoAmI()}
        };
        
        public UpdateService(IBotService botService, UserContext userContext, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _userContext = userContext;    // TODO: заменить на IRepository<User>. Удобнее тестировать будет
            _logger = logger;
        }

        public async Task Update(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Type == MessageType.Text && message.Text.StartsWith("/"))
                {
                    await HandleComands(message);
                }
                else
                {
                    await HandleMessages(message);
                }
            }
        }
        
        private async Task HandleComands(Message message)
        {
            var messageComandAndArgs = message.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var commandName = messageComandAndArgs.FirstOrDefault();
            var commandArgs = messageComandAndArgs.Skip(1);

            _logger.LogInformation($"Получена команда {commandName} с аргументами {commandArgs} из диалога: {message.Chat.Id}");

            if (!_botCommands.ContainsKey(commandName))
            {
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, 
                    $"Ошибка выполнения команды {commandName}: неизвестная команда");
                return;
            }
            
            await _botCommands[commandName].Execute(commandArgs, _botService, _userContext, message);
        }
        
        private async Task HandleMessages(Message message)
        {
            // TODO: скорее всего этот метод будет использоваться для разных типов сообщений, а не только текста
            // стоит подумать, как вынести обработку сообщений по аналогии с обработкой команд
            _logger.LogInformation("Получено сообщение из диалога {0}", message.Chat.Id);

            await _botService.Client.SendTextMessageAsync(message.Chat.Id, 
                $"Ошибка отправки сообщения: эта функция еще не реализована");
        }
    }
}