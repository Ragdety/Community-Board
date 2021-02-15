using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
