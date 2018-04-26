using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegramAggregator.Services.NotificationsService;
using VkConnector.Model.Messages;

namespace TelegramAggregator.Controllers
{
    /// <summary>
    /// Получение новых сообщений для пользователей
    /// </summary>
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        /// <summary>
        /// Отправка нового сообщения пользователю
        /// </summary>
        /// <param name="id">Id телеграмм-пользователя или его чата</param>
        /// <param name="recievedMessage">Доставляемое сообщение</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> Post(long id, [FromBody] RecievedMessage recievedMessage)
        {
            await _notificationsService.SendNotificationToUser(id, recievedMessage);
            return Ok();
        }
    }
}