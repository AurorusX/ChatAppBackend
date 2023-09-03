using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Serializable]
    public class ChatMessage
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public string ChatId { get; set; }
    }
}
