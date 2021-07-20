using System;
using CommunityBoard.Core.Enums;

namespace CommunityBoard.Core.DTOs.Responses
{
    public class AnnouncementResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public AnnouncementType Type { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
    }
}