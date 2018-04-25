using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types.InputFiles;
using TelegramAggregator.Data.Entities;
using VkNet;
using VkNet.Model.RequestParams;

namespace TelegramAggregator.Services.MessagesNotify
{
    public class MessageNotify : IMessageNotify
    {
        private const int LongPoolWait = 20;
        private const int LongPoolMode = 2;
        private const int LongPoolVersion = 2;
        
        private readonly IBotService _botService;
        
        public MessageNotify(IBotService botService)
        {
            _botService = botService;
        }
        
        public async void StartSubsribe(BotUser botUser)
        {
            var api = new VkApi();
            await api.AuthorizeAsync(new ApiAuthParams()
            {
                AccessToken = botUser.VkAccount.AcessToken
            });

            Task.Factory.StartNew(async () => { await NotifyNewUpdates(botUser, api); });
        }

        private async Task NotifyNewUpdates(BotUser botUser, VkApi api)
        {
            var client = new HttpClient();
            var longPollServer = api.Messages.GetLongPollServer();
            var ts = longPollServer.Ts;

            while (true)
            {
                var updateResponse = await client
                    .GetAsync($"https://{longPollServer.Server}?act=a_check&key={longPollServer.Key}&ts={ts}&wait={LongPoolWait}&mode={LongPoolMode}&version={LongPoolVersion}");
                var jsoned = await updateResponse.Content.ReadAsStringAsync();
                var updates = JsonConvert.DeserializeObject<JObject>(value: jsoned);

                var longPollHistory = await api.Messages.GetLongPollHistoryAsync(@params: new MessagesGetLongPollHistoryParams
                {
                    Ts = ts
                });

                foreach (var message in longPollHistory.Messages)
                {
                    if (message.Out != null && message.Out.Value)
                        await _botService.Client.SendTextMessageAsync(botUser.TelegramChatId, $"Отправлено сообщение {message.UserId ?? message.ChatId}: \r\n {message.Body}");
                    else
                        await _botService.Client.SendTextMessageAsync(botUser.TelegramChatId, $"Получено сообщение от {message.UserId ?? message.ChatId}: \r\n {message.Body}");
                }

                ts = updates[propertyName: "ts"].ToObject<ulong>();
            }
        }
    }
}