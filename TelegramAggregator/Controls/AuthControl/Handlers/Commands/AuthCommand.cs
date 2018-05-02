using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using TelegramAggregator.Controls.AuthControl.Common;
using TelegramAggregator.Model.Entities;
using TelegramAggregator.Model.Extensions;
using TelegramAggregator.Model.Repositories;
using VkNet;
using VkNet.Enums.Filters;

namespace TelegramAggregator.DialogsControl.Handlers.Commands
{
    public class AuthCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string ArgsInput { get; set; }
    }

    public class AuthCommand : CommandBase<AuthCommandArgs>
    {
        private readonly IBotUserRepository _botUserRepository;
        
        public AuthCommand(IBotUserRepository botUserRepository)
            : base(Constants.Command)
        {
            _botUserRepository = botUserRepository;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(IBot bot, Update update, AuthCommandArgs args)
        {
            if (args.ArgsInput.Split(" ").Length != 1)
            {
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, $"usage: {Constants.CommandFormat}");
            }

            var acessToken = args.ArgsInput;

            var botUser = _botUserRepository.GetByTelegramId(update.Message.Chat.Id);
            if (botUser == null)
            {
                botUser = new BotUser()
                {
                    TelegramChatId = update.Message.Chat.Id,
                    TelegramUserId = update.Message.Chat.Id
                };
            }

            try
            {
                var userInfo = botUser.AuthorizeVk(acessToken);
                _botUserRepository.Add(botUser);
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, $"Вы авторизованы как: {userInfo.FirstName} {userInfo.LastName}, id{userInfo.Id}");
            }
            catch (Exception e)
            {
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, e.Message);
            }
  
            return UpdateHandlingResult.Handled;
        }
    }
}