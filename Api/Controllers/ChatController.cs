using Api.DTOs;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [AllowAnonymous]
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
        //Injected  ChatService here
        private readonly ChatService _chatService;
        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }



        [AllowAnonymous]
        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserDto model)
        {
            if (_chatService.AddUserToList(model.Name))
            {
                //204 code
				return NoContent();
			}

            return BadRequest("Name is taken, choose another");


           
        }


        [HttpGet("get-chat-messages")]
        public async Task<IActionResult> RetrieveMessagesAsync(string chatId)
        {
            try
            {
                var messages = await _chatService.GetChatMessagesAsync(chatId);

                if (messages.IsNullOrEmpty())
                {
                    chatId = await _chatService.ConvertToOtherOrderAsync(chatId);
                    messages = await _chatService.GetChatMessagesAsync(chatId);

                }
                // 200 OK status code with the messages
                return Ok(messages);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
