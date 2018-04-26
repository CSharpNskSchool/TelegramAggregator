using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAggregator.Data.Entities;
using VkConnector.Client;
using VkConnector.Model.Messages;
using VkConnector.Model.Users;

namespace TelegramAggregator.Services.MessageTransferService
{
    public class MessageTransferService : IMessageTransferService
    {
        private readonly IBotService _botService;
        
        public MessageTransferService(IBotService botService)
        {
            _botService = botService;
        }


        public async Task Transfer(BotUser botUser, Message message)
        {
            var connector = new VkConnectorClient("http://localhost:5000");
            
            await connector.SendMessage(new TransmittedMessage()
            {
                AuthorizedSender = new AuthorizedUser()
                {
                    AccessToken = botUser.VkAccount.AcessToken
                },
                Body = new MessageBody(message.Text),
                Receiver = new ExternalUser(botUser.VkAccount.CurrentPeer)
            });
        }
    }
}