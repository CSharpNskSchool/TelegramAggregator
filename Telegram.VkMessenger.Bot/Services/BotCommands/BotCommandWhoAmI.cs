using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.VkMessenger.Bot.Services.BotCommands
{
    public class BotCommandWhoAmI : IBotCommand
    {
        public async Task Execute(IEnumerable<string> commandArgs, IBotService botService, Message message)
        {
            // TODO: тут должно быть получение информации об авторизации в Вконтакте, а не Телеграме
            await botService.Client.SendTextMessageAsync(message.Chat.Id, $"{message.From}");
        }
    }
}