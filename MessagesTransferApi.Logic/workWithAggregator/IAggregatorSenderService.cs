using CommunicationModels.Models;
using MessagesTransferApi.Data.Models;

namespace MessagesTransferApi.Logic
{
    public interface IAggregatorSenderService
    {
        void SendMessage(User user, RecievedMessage message);
    }
}
