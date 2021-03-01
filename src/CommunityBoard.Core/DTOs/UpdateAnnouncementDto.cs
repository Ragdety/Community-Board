using CommunityBoard.Core.Enums;

namespace CommunityBoard.Core.DTOs
{
    public class UpdateAnnouncementDto
    {
        public string Name { get; set; }
        public string Type { get; set; } 
        public string Description { get; set; }
        public byte[] Image { get; set; } 
    }
}
