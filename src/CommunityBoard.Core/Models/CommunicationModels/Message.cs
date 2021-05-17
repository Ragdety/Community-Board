using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityBoard.Core.Models.CommunicationModels
{
	public class Message
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }
		
		[Required]
		public DateTime Timestamp { get; set; }

		[Required]
		public string UserName { get; set; }

		public int ChatId { get; set; }

		public virtual Chat Chat { get; set; }
	}
}