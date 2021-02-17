using System.ComponentModel.DataAnnotations;

namespace CommunityBoard.Core.Models
{
    public class AnnouncementImage
    {
        [Required]
        public int AnnouncementId { get; private set; }

        [Required]
        public string ImageTitle { get; set; }

        [Required]
        public byte[] ImageData { get; set; }
    }
}
