using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.MessageTransferService
{
    public interface IMessageTransferService
    {
        Task Transfer(BotUser botUser, Message message);
    }
}