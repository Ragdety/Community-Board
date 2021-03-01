using CommunityBoard.Core.Enums;

namespace CommunityBoard.Core.DTOs
{
    public class CreateAnnouncementDto
    {
        public string Name { get; set; }
        public AnnouncementType Type { get; set; } = AnnouncementType.Other;
        public string Description { get; set; }
        public byte[] Image { get; set; } = null;
    }
}