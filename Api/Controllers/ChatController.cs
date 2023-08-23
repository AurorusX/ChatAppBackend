using Api.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
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




        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserDto model)
        {
            if (_chatService.AddUserToLIst(model.Name))
            {
                //204 code
				return NoContent();
			}

            return BadRequest("Name is taken, choose another");


           
        }

    }
}
