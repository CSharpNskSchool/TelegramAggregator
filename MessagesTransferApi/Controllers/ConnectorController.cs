using System.Threading.Tasks;
using MessagesTransferApi.Data.Contexts;
using MessagesTransferApi.Data.Models;
using MessagesTransferApi.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommunicationModels.Models;
using System;
using MessagesTransferApi.Client;

namespace MessagesTransferApi.Controllers
{
    [Produces("application/json")]
    [Route("v0/api/[Controller]")]
    public class ConnectorController : Controller
    {
        private readonly IAggregatorSenderService _aggregatorSender;
        private readonly DataContext _context;

        public ConnectorController(DataContext context, IAggregatorSenderService aggregatorSender)
        {
            _context = context;
            _aggregatorSender = aggregatorSender;
        }

        /// <summary>
        ///     Получить список всех коннекторов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetConnectors()
        {
            return Json(_context.Connectors);
        }

        /// <summary>
        ///     Добавить коннектор к МТА
        /// </summary>
        /// <param name="connectorData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AttachConnector([FromBody] ConnectorData connectorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!Uri.TryCreate(connectorData.Url, UriKind.RelativeOrAbsolute, out var uri))
            {
                return BadRequest("Неверный URL");
            }

            var connector = new Connector
            {
                NetworkName = connectorData.NetworkName,
                Url = connectorData.Url
            };

            _context.Connectors.Add(connector);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        ///     Отправка сообщения в аггрегатор
        /// </summary>
        /// <param name="messageData">данные для отправки</param>
        /// <param name="networkName">название соц.сети</param>
        /// <param name="id">id получателя </param>
        /// <returns></returns>
        [HttpPost]
        [Route("Messages/{id:int}")]
        public async Task<IActionResult> SendMessage(int id, [FromQuery] string networkName, [FromBody] RecievedMessage message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("Неверный id пользователя");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == id && a.PlatformName == networkName);

            if(account == null)
            {
                return NotFound("Пользователь не привязан к данной сети");
            }

            try
            {
                _aggregatorSender.SendMessage(user, message);
                return Ok();
            }
            catch
            {
                return NotFound(new { ErrorMessage = "Не удалось отправить сообщение" });
            }
        }
    }
}