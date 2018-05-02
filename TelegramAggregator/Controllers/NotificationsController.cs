using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TelegramAggregator.Controllers
{
    /// <summary>
    ///     Получение новых сообщений для пользователей
    /// </summary>
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        /// <summary>
        ///     Отправка уведомлений пользователю
        /// </summary>
        /// <param name="id">Id телеграмм-пользователя или его чата</param>
        /// <param name="recievedMessage">Доставляемое сообщение</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> Post(long id)
        {
            return Ok();
        }
    }
}