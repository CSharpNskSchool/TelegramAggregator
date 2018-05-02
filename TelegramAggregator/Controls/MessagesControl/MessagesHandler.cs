using System.Threading.Tasks;
using CommunicationModels.Models;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAggregator.Model.Repositories;
using VkConnector.Client;

namespace TelegramAggregator.Controls.MessagesControl
{
    public class MessagesHandler : IUpdateHandler
    {
        private readonly IBotUserRepository _botUserRepository;

        public MessagesHandler(IBotUserRepository botUserRepository)
        {
            _botUserRepository = botUserRepository;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            return update.Type == UpdateType.Message;
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var message = update.Message;
            var botUser = _botUserRepository.GetByTelegramId(message.Chat.Id);
            var connector = new ConnectorsClient("http://localhost:5000");
            
            await connector.SendMessage(new TransmittedMessage()
            {
                AuthorizedSender = new AuthorizedUser()
                {
                    AccessToken = botUser.VkAccount.AcessToken
                },
                Message = new CommunicationModels.Models.Message()
                {
                    Body = new MessageBody(message.Text),
                    Receiver = new ExternalUser(botUser.VkAccount.CurrentPeer)
                }
            });
            
            await bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Cообщение доставлено",
                replyToMessageId: message.MessageId);
            
            return UpdateHandlingResult.Handled;
        }
    }
}