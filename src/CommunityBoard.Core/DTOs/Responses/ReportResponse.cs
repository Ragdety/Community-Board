using System;

namespace CommunityBoard.Core.DTOs.Responses
{
	public class ReportResponse
	{
		public int Id { get; set; }
		public string ReportCause { get; set; }
		public string ReportDescription { get; set; }
		public DateTime ReportDate { get; set; }
		public int AnnouncementId { get; set; }
	}
}