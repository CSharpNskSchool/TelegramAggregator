using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramAggregator.Services
{
    public interface IUpdateService
    {
        Task Update(Update update);
    }
}