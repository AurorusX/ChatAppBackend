using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
	public class ChatService


	{
       
        private readonly ChatDbContext _chatdbcontext;
        public ChatService(ChatDbContext ChatDbContext)
        {
           
            _chatdbcontext = ChatDbContext;
        }


        private static readonly Dictionary<string,string> Users = new Dictionary<string,string>();


		public bool AddUserToLIst(string addedUser)
		{
			lock(Users)
			{
				foreach(var user in Users)
				{
					if(user.Key.ToLower() == addedUser.ToLower())
					{
						return false;
					}
				}
				Users.Add(addedUser, null);
				return true;
			}
		}

		public void AddUserConnectionId(string user,string connectionId)
		{
			lock (Users)
			{
				if (Users.ContainsKey(user))
				{
					Users[user] = connectionId;
				}
			}
		}

		public string GetUserByConnectionId(string connectionId)
		{
			lock(Users)
			{
				return Users.Where(x=>x.Value == connectionId).Select(x=>x.Key).FirstOrDefault();
			}
		}


		public string GetConnectionIdByUser(string user)
		{
			lock (Users)
			{
				return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
			}
		}

		public void RemoveUserFromList(string user)
		{
			lock( Users)
			{
				if (Users.ContainsKey(user))
				{
					Users.Remove(user);
				}
			}
		}

		public string[] GetOnlineUsers()
		{
			lock (Users)
			{
				return Users.OrderBy(x=> x.Key).Select(x=>x.Key).ToArray();
			}
		}



        public Task<List<ChatMessage>>GetChatMessagesAsync(string chatId)
        {
            return _chatdbcontext.ChatMessages
                .Where(message => message.ChatId == chatId)
                .ToListAsync();
        }

        public Task<string> ConvertToOtherOrderAsync(string input)
        {
            // Split the input string using the '-' separator
            string[] parts = input.Split('-');

            // Check if there are exactly two parts
            if (parts.Length == 2)
            {
                // Swap the order of the parts and concatenate with '-'
                string result = $"{parts[1]}-{parts[0]}";
                return Task.FromResult(result);
            }
            else
            {
                // Handle the case where the input is not in the expected format
                return Task.FromResult("Invalid input format");
            }
        }








    }
} 
