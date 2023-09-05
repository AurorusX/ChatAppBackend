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

        public ChatService(ChatDbContext chatDbContext)
        {
            _chatDbContext = chatDbContext;
        }

        public async Task<bool> AddUserToListAsync(string username)
        {
            // Check if the user already exists in the database
            var existingUser = await _chatDbContext.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();

            if (existingUser == null)
            {
                var newUser = new User
                {
                    Name = username,
                    //ConnectionId = connectionId
                };

                _chatDbContext.Users.Add(newUser);
                await _chatDbContext.SaveChangesAsync();
                return true;

            }
            else
            {
                return false;
            }

           
        }

        public async Task AddUserConnectionIdAsync(string username, string connectionId)
        {
            var user = await _chatDbContext.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                user.ConnectionId = connectionId;
                await _chatDbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetUserByConnectionIdAsync(string connectionId)
        {
            var user = await _chatDbContext.Users
                .Where(u => u.ConnectionId == connectionId)
                .FirstOrDefaultAsync();

            return user?.Name;
        }

        public async Task<string> GetConnectionIdByUserAsync(string username)
        {
            var user = await _chatDbContext.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();

            return user?.ConnectionId;
        }

        public async Task RemoveUserFromListAsync(string username)
        {
            var user = await _chatDbContext.Users
                .Where(u => u.Name == username)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                _chatDbContext.Users.Remove(user); // Remove the user
                await _chatDbContext.SaveChangesAsync(); // Save changes to the database
            }
        }

        public async Task<string[]> GetOnlineUsersAsync()
        {
            var onlineUsers = await _chatDbContext.Users
                .Where(u => u.ConnectionId != null)
                .Select(u => u.Name)
                .ToArrayAsync();

            return onlineUsers;
        }

        // Other methods follow the same pattern...

        public async Task<List<ChatMessage>> GetChatMessagesAsync(string chatId)
        {
            return await _chatDbContext.ChatMessages
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
