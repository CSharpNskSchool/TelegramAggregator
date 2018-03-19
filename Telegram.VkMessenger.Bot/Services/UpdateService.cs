using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.VkMessenger.Bot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;

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

            // TODO: Тут должен быть какая-нибудь фабрика с методами для всех команд,
            //                             чтобы получать нужный по названию команды.
            // Пока у меня только одна команда - задать acess_token, поэтому сделаю костыль
            // Вообще надо что-то по типу команд из домашки с брэйнфаком:
            if (commandName == "/login")
            {
                if (commandArgs.Count() != 1)
                {
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, $"Формат команды: /login <acess_token>");
                    return;
                }

                var vkAcessToken = commandArgs.First();
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, $"Вы будете авторизаваны с токеном {vkAcessToken}");
                // TODO: записать указанный токен в бд
            }
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