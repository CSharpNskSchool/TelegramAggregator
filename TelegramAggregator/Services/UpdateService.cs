using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAggregator.Data.Entities;
using TelegramAggregator.Data.Repositories;
using TelegramAggregator.Services.BotCommands;
using TelegramAggregator.Services.CommandsHandler;
using TelegramAggregator.Services.MessagesTrasfer;

namespace TelegramAggregator.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly IBotUserRepository _botUserRepository;
        private readonly IMessageTransfer _messageTransfer;
        private readonly ICommandsHandler _commandsHandler;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(
            IBotService botService, 
            IBotUserRepository botUserRepository,
            IMessageTransfer messageTransfer,
            ICommandsHandler commandsHandler,
            ILogger<UpdateService> logger)
        {
            _botService = botService;
            _botUserRepository = botUserRepository;
            _messageTransfer = messageTransfer;
            _commandsHandler = commandsHandler;
            _logger = logger;
        }

        public async Task Update(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var botUser = GetRequestingUserOrRegisterNew(message.From.Id);

                if (message.Type == MessageType.Text && message.Text.StartsWith("/"))
                {
                    _logger.LogInformation($"Получена команда из диалога {message.Chat.Id}");

                    await _commandsHandler.Transfer(botUser, message);
                }
                else
                {
                    _logger.LogInformation($"Получено сообщение из диалога {message.Chat.Id}");

                    await _messageTransfer.Transfer(botUser, message);
                }
            }
        }

        private BotUser GetRequestingUserOrRegisterNew(int telegramId)
        {
            var botUser = _botUserRepository.GetByTelegramId(telegramId);
            
            if (botUser == null)
            {
                botUser = new BotUser
                {
                    TelegramId = telegramId
                };

                _botUserRepository.Add(botUser);
            }

            return botUser;
        }
    }
}