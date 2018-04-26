using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkConnector.Model;
using VkConnector.Services;

namespace VkConnector.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SubscribeController : Controller
    {
        private readonly IUpdatesListener _updatesListener;

        public SubscribeController(IUpdatesListener updatesListener)
        {
            _updatesListener = updatesListener;
        }

        /// <summary>
        ///     Установка WebHook для получения обновлений пользователя
        /// </summary>
        /// <response code="200">WebHook успешно установлен</response>
        /// <response code="400">Установка WebHook не удалась</response>
        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionModel subscriptionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseResult
                {
                    IsOk = false,
                    Description = string.Join("\r\n",
                        ModelState.Values.SelectMany(entry => entry.Errors).Select(error => error.ErrorMessage))
                });
            }

            _updatesListener.StartListening(subscriptionModel);

            return Ok(new ResponseResult
            {
                IsOk = true,
                Description = $"WebHook установлен на адрес {subscriptionModel.Url}"
            });
        }
    }
}