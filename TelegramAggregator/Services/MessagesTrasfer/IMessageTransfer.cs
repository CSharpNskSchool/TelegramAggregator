using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.MessagesTrasfer
{
    public interface IMessageTransfer
    {
        Task Transfer(BotUser botUser, Message message);
    }
}