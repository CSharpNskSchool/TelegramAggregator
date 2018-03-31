using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.CommandsHandler
{
    public interface ICommandsHandler
    {
        Task Transfer(BotUser botUser, Message commandMessage);
    }
}