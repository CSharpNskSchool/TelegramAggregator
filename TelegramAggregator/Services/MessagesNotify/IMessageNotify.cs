using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Services.MessagesNotify
{
    public interface IMessageNotify
    {
        void StartSubsribe(BotUser botUser);
    }
}