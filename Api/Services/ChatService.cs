using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class ChatService
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        private readonly object usersLock = new object();

        public ChatService(ChatDbContext chatDbContext)
        {
            _chatDbContext = chatDbContext;
        }

        public bool AddUserToList(string addedUser)
        {
            lock (usersLock)
            {
                // Normalize the addedUser by removing spaces and converting to lowercase
                var normalizedUser = addedUser.Replace(" ", "").ToLower();

                // Check if the normalized user already exists in the Users dictionary
                if (Users.ContainsKey(normalizedUser))
                {
                    return false; // User with this normalized name already exists
                }

                // Add the normalized user to the Users dictionary
                Users.Add(normalizedUser, null);
                return true;
            }
        }

        public void AddUserConnectionId(string user, string connectionId)
        {
            lock (usersLock)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (usersLock)
            {
                return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (usersLock)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        public void RemoveUserFromList(string user)
        {
            lock (usersLock)
            {
                if (Users.ContainsKey(user))
                {
                    Users.Remove(user);
                }
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (usersLock)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }

        public Task<List<ChatMessage>> GetChatMessagesAsync(string chatId)
        {
            return _chatDbContext.ChatMessages
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
