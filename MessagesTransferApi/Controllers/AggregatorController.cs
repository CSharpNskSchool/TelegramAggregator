using System;
using System.Linq;
using System.Threading.Tasks;
using CommunicationModels.Models;
using MessagesTransferApi.Data.Contexts;
using MessagesTransferApi.Data.Models;
using MessagesTransferApi.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VkConnector.Client;

namespace MessagesTransferApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class AggregatorController : Controller
    {
        private readonly DataContext _context;

        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly HttpRequest request;

        public AggregatorController(
            DataContext context,
            ITokenGeneratorService tokenGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            request = httpContextAccessor.HttpContext.Request;
        }

        /// <summary>
        ///     Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Users")]
        public IActionResult GetUsers()
        {
            return Json(_context.Users);
        }

        /// <summary>
        ///     Получить все аккаунты
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Accounts")]
        public IActionResult GetAccounts()
        {
            return Json(_context.Accounts);
        }

        /// <summary>
        ///     Добавить пользователя
        /// </summary>
        /// <param name="userData"> данные о пользователе </param>
        /// <returns></returns>
        [HttpPost]
        [Route("Users")]
        public async Task<IActionResult> AddUser([FromBody] UserData userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existedUser = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Login == userData.Login);

            if (existedUser != null)
            {
                return BadRequest("Логин уже существует");
            }

            var userToken = _tokenGenerator.GenerateToken(userData.Login);

            var user = new User
            {
                Login = userData.Login,
                FeedbackUrl = userData.Url,
                UserToken = userToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Json(new {generatedToken = userToken});
        }


        [HttpPost]
        [Route("Webhook")]
        public async Task<IActionResult> AttachWebhook([FromBody] string url, [FromQuery] string userToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.UserToken == userToken);

            if (user == null)
            {
                return NotFound("неверный токен");
            }

            user.FeedbackUrl = url;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        ///     Подписаться на обновления
        /// </summary>
        /// <param name="account">данные из соц.сети (как я понял)</param>
        /// <param name="userToken">токен выделенный при добавлении пользователя</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Users/Accounts")]
        public async Task<IActionResult> AttachAccount([FromBody] Account account, [FromQuery] string userToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context
                .Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.UserToken == userToken);

            if (user == null)
            {
                return NotFound("Неверный токен");
            }

            var connector = _context
                .Connectors
                .FirstOrDefault(c => c.NetworkName == account.NetworkName);

            if (connector == null)
            {
                return NotFound("Не существует запрашиваемой социальной сети");
            }

            var networkAuthData = new NetworkAuthData
            {
                UserId = user.Id,
                User = user,
                PlatformName = account.NetworkName,
                AccessToken = account.AccessToken
            };

            await new ConnectorsClient(connector.Url)
                    .SetWebHook(new SubscriptionModel()
                    {
                        Url = new Uri($"{request.Scheme}://{request.Host.ToUriComponent()}/Connector/Messages/{user.Id}?networkName={account.NetworkName}"),
                        User = new AuthorizedUser()
                        {
                            AccessToken = account.AccessToken
                        }
                    });

            user.Accounts.Add(networkAuthData);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        ///     Отправка сообщения в коннектор
        /// </summary>
        /// <param name="transmittedData"> данные для пересылки </param>
        /// <param name="userToken">пользовательский токен</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Messages")]
        public async Task<IActionResult> SendMessage([FromBody] AggregatorMessage transmittedData,
            [FromQuery] string userToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context
                .Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.UserToken == userToken);

            if (user == null)
            {
                return NotFound("Неверный токен");
            }

            var accessToken = user
                .Accounts
                .FirstOrDefault(a => a.PlatformName == transmittedData.NetworkName)
                .AccessToken;

            if (accessToken == null)
            {
                return BadRequest("Для данной сети токен даступа не найден");
            }

            var connector = await _context
                .Connectors
                .FirstOrDefaultAsync(c => c.NetworkName == transmittedData.NetworkName);

            await new ConnectorsClient(connector.Url).SendMessage(new TransmittedMessage
            {
                AuthorizedSender = new AuthorizedUser {AccessToken = accessToken},
                Message = transmittedData.Message
            });

            return Ok();
        }
    }
}