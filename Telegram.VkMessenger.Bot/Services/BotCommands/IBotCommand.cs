using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.VkMessenger.Bot.Models;

namespace Telegram.VkMessenger.Bot.Services.BotCommands
{
    public interface IBotCommand
    {
        Task Execute(IEnumerable<string> commandArgs, IBotService botService, UserContext userContext, Message message);
    }
}