using System.Net.Http;
using System.Threading.Tasks;

namespace MessagesTransferApi.Logic
{
    public class HttpSender
    {
        public async Task SendPostRequestAsync(HttpContent content, string url)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsync(url, content);
            }
        }
    }
}
