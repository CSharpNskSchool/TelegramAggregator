using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAggregator.Data.Entities;
using TelegramAggregator.Data.Repositories;
using TelegramAggregator.Services.BotCommands;
using TelegramAggregator.Services.CommandsHandler;
using TelegramAggregator.Services.MessageTransferService;

namespace TelegramAggregator.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly IBotUserRepository _botUserRepository;
        private readonly IMessageTransferService _messageTransferService;
        private readonly ICommandsHandler _commandsHandler;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(
            IBotService botService, 
            IBotUserRepository botUserRepository,
            IMessageTransferService messageTransferService,
            ICommandsHandler commandsHandler,
            ILogger<UpdateService> logger)
        {
            _botService = botService;
            _botUserRepository = botUserRepository;
            _messageTransferService = messageTransferService;
            _commandsHandler = commandsHandler;
            _logger = logger;
        }

        public async Task Update(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var botUser = GetRequestingUserOrRegisterNew(message.From.Id, message.Chat.Id);

                if (message.Type == MessageType.Text && message.Text.StartsWith("/"))
                {
                    _logger.LogInformation($"Получена команда из диалога {message.Chat.Id}: {message.Text}");

                    await _commandsHandler.Transfer(botUser, message);
                }
                else
                {
                    _logger.LogInformation($"Получено сообщение из диалога {message.Chat.Id}: {message.Text}");

                    await _messageTransferService.Transfer(botUser, message);
                }
            }
        }

        private BotUser GetRequestingUserOrRegisterNew(int telegramUserId, long telegramChatId)
        {
            var botUser = _botUserRepository.GetByTelegramId(telegramUserId);
            
            if (botUser == null)
            {
                botUser = new BotUser
                {
                    TelegramUserId = telegramUserId,
                    TelegramChatId = telegramChatId
                };

                _botUserRepository.Add(botUser);
            }

            return botUser;
        }
    }
}