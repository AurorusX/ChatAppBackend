using Api.DTOs;
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
            await Clients.Caller.SendAsync("UserConnected");
        }

		public override async Task OnDisconnectedAsync(Exception ex )
		{
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AscendantChat");
			var user =_chatservice.GetUserByConnectionId(Context.ConnectionId);
			_chatservice.RemoveUserFromList(user);
			await DisplayOnlineUsers();
			await base.OnDisconnectedAsync(ex);

			
		}



		public  async Task AddUserConnectionId(string name)
		{
			_chatservice.AddUserConnectionId(name,Context.ConnectionId);
			await DisplayOnlineUsers();

		}

		private async Task DisplayOnlineUsers()
		{
			var usersOnline = _chatservice.GetOnlineUsers();
			await Clients.Groups("AscendantChat").SendAsync("usersOnline", usersOnline);
		}

		private async Task ReceiveMessage(MessageDto message)
		{
			
			await Clients.Groups("AscendantChat").SendAsync("NewMessage", message);
		}
	}
}
