using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MessageTransferBot.Services
{
    public interface IUpdateService
    {
        Task Update(Update update);
    }
}