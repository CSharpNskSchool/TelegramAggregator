using System.Collections.Generic;
using TelegramAggregator.Data.Entities;

namespace TelegramAggregator.Data.Repositories
{
    public interface IBotUserRepository
    {
        IEnumerable<BotUser> Users { get; }
        BotUser Get(int id);
        BotUser GetByTelegramId(int telegramId);
        void Add(BotUser botUser);
    }
}