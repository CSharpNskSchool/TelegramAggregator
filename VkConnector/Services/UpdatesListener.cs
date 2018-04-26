using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkConnector.Model;
using VkConnector.Model.Messages;
using VkConnector.Model.Users;
using VkNet;
using VkNet.Model.RequestParams;

namespace VkConnector.Services
{
    public class UpdatesListener : IUpdatesListener
    {
        private const int LongPoolWait = 20;
        private const int LongPoolMode = 2;
        private const int LongPoolVersion = 2;

        public async Task StartListening(SubscriptionModel subscriptionModel)
        {
            var api = new VkApi();
            await api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = subscriptionModel.User.AccessToken
            });

            Task.Factory.StartNew(async () => { await NotifyNewUpdates(subscriptionModel, api); });
        }

        public void StopListening(SubscriptionModel subscriptionModel)
        {
            throw new NotImplementedException();
        }

        private async Task NotifyNewUpdates(SubscriptionModel subscriptionModel, VkApi api)
        {
            var client = new HttpClient();
            var longPollServer = api.Messages.GetLongPollServer();
            var ts = longPollServer.Ts;

            while (true)
            {
                var updateResponse = await client
                    .GetAsync(
                        $"https://{longPollServer.Server}?act=a_check&key={longPollServer.Key}&ts={ts}&wait={LongPoolWait}&mode={LongPoolMode}&version={LongPoolVersion}");
                var jsoned = await updateResponse.Content.ReadAsStringAsync();
                var updates = JsonConvert.DeserializeObject<JObject>(jsoned);

                var longPollHistory = await api.Messages.GetLongPollHistoryAsync(new MessagesGetLongPollHistoryParams
                {
                    Ts = ts
                });

                foreach (var message in longPollHistory.Messages)
                {
                    SendToWebHook(subscriptionModel.Url,
                        new RecievedMessage(
                            message.ChatId ?? -1,
                            new ExternalUser(message.UserId ?? -1),
                            !message.Out ?? false,
                            new MessageBody(message.Body)));
                }

                ts = updates["ts"].ToObject<ulong>();
            }
        }

        private void SendToWebHook(Uri url, RecievedMessage message)
        {
            Console.WriteLine($"\r\n\r\nПолучено новое сообщние от {message.Sender.Id}: {message.Body.Text} \r\n\r\n");
            var client = new HttpClient();
            var toSend = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var a = client.PostAsync(url, toSend);
            Console.WriteLine(url);
            Console.WriteLine($"Код ответа: {a.Result.StatusCode}");
        }
    }
}