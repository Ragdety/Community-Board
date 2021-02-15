using CommunityBoard.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommunityBoard.Core.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(180)]
        public string Name { get; set; }

        [Required]
        public AnnouncementType Type { get; set; }

        [Required]
        [MaxLength(600)]
        public string Description { get; set; }

        public AnnouncementImage Image { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        [Required]
        public User User { get; set; }
    }
}
