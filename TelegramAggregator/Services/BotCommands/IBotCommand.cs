using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.BotCommands
{
    public interface IBotCommand
    {
        Task Execute(IEnumerable<string> commandArgs, IBotService botService, BotUser botUser, Message message);
    }
}