using Telegram.Bot;

namespace TelegramAggregator.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
        BotConfiguration Configuration { get; }
    }
}