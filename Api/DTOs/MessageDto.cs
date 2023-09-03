using System;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
	[Serializable]
	public class MessageDto
	{
		[Required]
		public string From { get; set; }
		public string To { get; set; }
		[Required]
		public string Content { get; set; }

		public string ChatId { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
