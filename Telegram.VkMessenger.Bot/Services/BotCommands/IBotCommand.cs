using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.VkMessenger.Bot.Services.BotCommands
{
    public interface IBotCommand
    {
        Task Execute(IEnumerable<string> commandArgs, IBotService botService, Message message);
    }
}