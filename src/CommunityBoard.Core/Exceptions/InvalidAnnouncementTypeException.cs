using System;

namespace CommunityBoard.Core.Exceptions
{
	public class InvalidAnnouncementTypeException : Exception
	{
		public InvalidAnnouncementTypeException() :
			base("Announcement type not supported") { }
	}
}