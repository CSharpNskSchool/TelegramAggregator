using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;
using TelegramAggregator.Services.MessagesNotify;

namespace TelegramAggregator.Services.BotCommands
{
    public interface IBotCommand
    {
        Task Execute(IEnumerable<string> commandArgs, IBotService botService, IMessageNotify messageNotify,
            BotUser botUser, Message message);
    }
}