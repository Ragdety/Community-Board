using CommunityBoard.Core.Models.CoreModels;
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

		public DateTime Date { get; set; }

		[Required]
		public string UserName { get; set; }

		public int UserId { get; set; }

		public virtual User Sender { get; set; }
	}
}