using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunicationModels.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkNet;
using VkNet.Model.Attachments;
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

            Task.Factory.StartNew(async () => await NotifyNewUpdates(subscriptionModel, api));
        }

        public bool StopListening(SubscriptionModel subscriptionModel)
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
                    SendMessageToWebHook(subscriptionModel, message);
                }

                ts = updates["ts"].ToObject<ulong>();
            }
        }

        private void SendMessageToWebHook(SubscriptionModel subscriptionModel, VkNet.Model.Message message)
        {
            SendToWebHook(
                subscriptionModel.Url,
                new RecievedMessage(
                    chatId: message.ChatId ?? -1,
                    sender: new ExternalUser(message.UserId ?? -1),
                    isIncoming: !message.Out ?? false,
                    isForwarded: !message.Out.HasValue,
                    message: new Message()
                    {
                        Body = new MessageBody(message.Body),
                        Attachments = AssemblyReceivedAttachments(message.Attachments)
                    })
            );

            foreach (var forwardedMessage in message.ForwardedMessages)
            {
                SendMessageToWebHook(subscriptionModel, forwardedMessage);
            }
        }

        private IEnumerable<MessageAttachment> AssemblyReceivedAttachments(ReadOnlyCollection<Attachment> messageAttachments)
        {
            foreach (var attachment in messageAttachments)
            {
                yield return new MessageAttachment()
                {
                    AttachmentType = MessageAttachment.Type.Undifined
                };
            }
        }

        private void SendToWebHook(Uri url, RecievedMessage message)
        {
            var client = new HttpClient();
            var toSend = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            var a = client.PostAsync(url, toSend);

            Console.WriteLine($"\r\n\r\nПолучено новое сообщние от {message.Sender.Id}: {message.Message.Body.Text} \r\n\r\n");
            Console.WriteLine(url);
            Console.WriteLine($"Код ответа: {a.Result.StatusCode}");
        }
    }
}