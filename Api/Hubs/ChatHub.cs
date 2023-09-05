using Api.DTOs;
using Api.Models;
using Api.Services;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Api.Hubs
{
	public class ChatHub : Hub
	{
        private readonly ChatService _chatservice;
        private readonly ChatDbContext _chatdbcontext;
        public ChatHub(ChatService chatService, ChatDbContext ChatDbContext)
        {
            _chatservice = chatService;
            _chatdbcontext = ChatDbContext;



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

		public async Task ReceiveMessage(MessageDto message)


		{
            message.ChatId = "AscendantChat";
			message.Timestamp = DateTime.Now;
           
            await Clients.Group("AscendantChat").SendAsync("NewMessage", message);
            await SaveMessageToDatabase(message);
        }

		public async Task CreatePrivateChat(MessageDto message)
		{
			string privategroupname = GetPrivateGroupName(message.From,message.To);
			await Groups.AddToGroupAsync(Context.ConnectionId, privategroupname);
			var toConnectionId = _chatservice.GetConnectionIdByUser(message.To);
			await Groups.AddToGroupAsync(toConnectionId, privategroupname);

            message.ChatId = privategroupname;
            

            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
            await SaveMessageToDatabase(message);
        }

		public async Task ReceivePrivateMessage(MessageDto message)
		{
			string privategroupname = GetPrivateGroupName(message.From, message.To);
			message.ChatId = privategroupname;
            message.Timestamp = DateTime.Now;

           

			await Clients.Group(privategroupname).SendAsync("NewPrivateMessage", message);
            await SaveMessageToDatabase(message);
        }

		public async Task RemovePrivateChat(string from, string to)
		{
			string privategroupname = GetPrivateGroupName(from, to);
			await Clients.Group(privategroupname).SendAsync("ClosePrivateChat");

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, privategroupname);
			var toConnectionId = _chatservice.GetConnectionIdByUser(to);
			await Groups.RemoveFromGroupAsync(toConnectionId, privategroupname);

		}


		private string GetPrivateGroupName(string from , string to) { 
			//from: shane to: edward   edward-shane
			var stringCompare =string.CompareOrdinal(from, to) < 0;
			return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
		}


        private async Task SaveMessageToDatabase(MessageDto message)
        {
            var newMessage = new ChatMessage
            {
                From = message.From,
				To = message.To,
                Content = message.Content,
                Timestamp = DateTime.UtcNow,
				ChatId = message.ChatId,
            };

            await _chatdbcontext.ChatMessages.AddAsync(newMessage);
            await _chatdbcontext.SaveChangesAsync();

           
        }

    }
}
