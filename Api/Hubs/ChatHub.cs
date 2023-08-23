using Api.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Api.Hubs
{
	public class ChatHub : Hub
	{
        private readonly ChatService _chatservice;
        public ChatHub(ChatService chatService)
        {
            _chatservice = chatService;
        }


        public override async Task OnConnectedAsync()
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, "AscendantChat");
            await Clients.Caller.SendAsync("Userconnected");
        }

		public override async Task OnDisconnectedAsync(Exception ex )
		{
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AscendantChat");
			await base.OnDisconnectedAsync(ex);
			
		}





	}
}
