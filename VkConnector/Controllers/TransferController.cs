using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkConnector.Extensions;
using VkConnector.Model;
using VkConnector.Model.Messages;

namespace VkConnector.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TransferController : Controller
    {
        /// <summary>
        ///     Отправка сообщения от имени пользователя
        /// </summary>
        /// <response code="200">Успешная отправка сообщения</response>
        /// <response code="400">Отправка сообщения не удалась</response>
        [HttpPost]
        public async Task<IActionResult> TransferMessage([FromBody] TransmittedMessage transmittedMessage)
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

            try
            {
                await transmittedMessage.Transfer();
            }
            catch (Exception e)
            {
                return NotFound(new ResponseResult
                {
                    IsOk = false,
                    Description = e.Message
                });
            }


            return Ok(new ResponseResult {IsOk = true, Description = "Сообщение отправлено"});
        }
    }
}