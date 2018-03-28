using Telegram.Bot;

namespace MessageTransferBot.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
        BotConfiguration Configuration { get; }
    }
}