using Telegram.Bot;

namespace Telegram.VkMessenger.Bot.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}