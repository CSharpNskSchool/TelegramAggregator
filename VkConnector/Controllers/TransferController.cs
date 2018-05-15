﻿using System;
using System.Linq;
using System.Threading.Tasks;
using CommunicationModels.Models;
using Microsoft.AspNetCore.Mvc;
using VkConnector.Extensions;
using VkConnector.Model;

namespace VkConnector.Controllers
{
    [Produces("application/json")]
    [Route("v0/api/[controller]")]
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
                    Description = string.Join(
                        "\r\n",
                        ModelState.Values.SelectMany(entry => entry.Errors).Select(error => error.ErrorMessage))
                });
            }

            try
            {
                await transmittedMessage.Transfer();
                return Ok(new ResponseResult
                {
                    IsOk = true,
                    Description = "Сообщение отправлено"
                });
            }
            catch (Exception e)
            {
                return NotFound(new ResponseResult
                {
                    IsOk = false,
                    Description = e.Message
                });
            }
        }
    }
}