using CommunicationModels.Models;
using System.Threading.Tasks;
using TelegramAggregator.Data.Entities;
using VkConnector.Client;

namespace TelegramAggregator.Services.MessageTransferService
{
    public class MessageTransferService : IMessageTransferService
    {
        private readonly IBotService _botService;
        
        public MessageTransferService(IBotService botService)
        {
            _botService = botService;
        }

        public async Task Transfer(BotUser botUser, Telegram.Bot.Types.Message message)
        {
            var connector = new ConnectorsClient("http://localhost:5000");
            
            await connector.SendMessage(new TransmittedMessage()
            {
                AuthorizedSender = new AuthorizedUser()
                {
                    AccessToken = botUser.VkAccount.AcessToken
                },
                Message = new Message()
                {
                    Body = new MessageBody(message.Text),
                    Receiver = new ExternalUser(botUser.VkAccount.CurrentPeer)
                }
            });
        }
    }
}