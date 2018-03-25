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
        private readonly ILogger<UpdateService> _logger;
        private readonly UserContext _userContext;

        public UpdateService(IBotService botService, UserContext userContext, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _userContext = userContext; // TODO: заменить на IRepository<User>. Удобнее тестировать будет
            _logger = logger;
        }

        public async Task Update(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Type == MessageType.Text && message.Text.StartsWith("/"))
                {
                    _logger.LogInformation($"Получена команда из диалога {message.Chat.Id}");

                    await BotCommandsExecutor.HandleComands(_botService, _userContext, message);
                }
                else
                {
                    _logger.LogInformation($"Получено сообщение из диалога {message.Chat.Id}");

                    await HandleMessages(message);
                }
            }
        }

        private async Task HandleMessages(Message message)
        {
            // TODO: скорее всего этот метод будет использоваться для разных типов сообщений, а не только текста
            // стоит подумать, как вынести обработку сообщений по аналогии с обработкой команд

            await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                "Ошибка отправки сообщения: эта функция еще не реализована");
        }
    }
}