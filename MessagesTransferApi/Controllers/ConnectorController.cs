using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessagesTransferApi.Data.Contexts;
using MessagesTransferApi.Logic;
using MessagesTransferApi.Models;
using MessagesTransferApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CommunicationModels.Models;

namespace MessagesTransferApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class ConnectorController : Controller
    {
        private readonly DataContext _context;
        private readonly IAggregatorSenderService _aggregatorSender;

        public ConnectorController(DataContext context, IAggregatorSenderService aggregatorSender)
        {
            this._context = context;
            this._aggregatorSender = aggregatorSender;
        }

        /// <summary>
        /// Получить список всех коннекторов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetConnectors() => Json(_context.Connectors);

        /// <summary>
        /// Добавить коннектор к МТА
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

            var connector = new Connector()
            {
                NetworkName = connectorData.NetworkName,
                Url = connectorData.Url
            };

            _context.Connectors.Add(connector);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Отправка сообщения в аггрегатор
        /// </summary>
        /// <param name="messageData">данные для отправки</param>
        /// <param name="networkName">название соц.сети</param>
        /// <param name="id">id получателя </param>
        /// <returns></returns>
        [HttpPost]
        [Route("Messages/{id:int}")]
        public async Task<IActionResult> SendMessage([FromBody] RecievedMessage message, [FromQuery] string networkName, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context
                .Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("Неверный токен");
            }

            _aggregatorSender.SendMessage(user, message);
            return Ok();
        }
    }
}