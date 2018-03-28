using System.Collections.Generic;
using System.Threading.Tasks;
using MessageTransferBot.Data.Entities;
using Telegram.Bot.Types;

namespace MessageTransferBot.Services.BotCommands
{
    public interface IBotCommand
    {
        Task Execute(IEnumerable<string> commandArgs, IBotService botService, BotUser botUser, Message message);
    }
}