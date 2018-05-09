using System.Net.Http;
using System.Text;
using CommunicationModels.Models;
using MessagesTransferApi.Data.Models;
using Newtonsoft.Json;

namespace MessagesTransferApi.Logic
{
    public class DirectAggregatorSenderService : IAggregatorSenderService
    {
        private readonly HttpSender sender = new HttpSender();

        public async void SendMessage(User user, RecievedMessage message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
            var content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");
            await sender.SendPostRequestAsync(content, user.FeedbackUrl);
        }
    }
}