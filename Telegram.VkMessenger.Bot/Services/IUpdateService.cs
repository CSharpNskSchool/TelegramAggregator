using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.VkMessenger.Bot.Services
{
    public interface IUpdateService
    {
        Task Update(Update update);
    }
}