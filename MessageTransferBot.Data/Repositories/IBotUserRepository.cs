using System.Collections.Generic;
using MessageTransferBot.Data.Entities;

namespace MessageTransferBot.Data.Repositories
{
    public interface IBotUserRepository
    {
        IEnumerable<BotUser> Users { get; }
        BotUser Get(int id);
        BotUser GetByTelegramId(int telegramId);
        void Add(BotUser botUser);
    }
}