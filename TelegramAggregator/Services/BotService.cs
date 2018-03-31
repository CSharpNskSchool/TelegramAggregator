using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace TelegramAggregator.Services
{
    public class BotService : IBotService
    {
        public BotService(IOptions<BotConfiguration> config)
        {
            Configuration = config.Value;
            Client = new TelegramBotClient(Configuration.BotToken);
        }

        public TelegramBotClient Client { get; }

        public BotConfiguration Configuration { get; }
    }
}