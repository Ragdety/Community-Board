using CommunityBoard.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityBoard.Core.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Type Required")]
        public AnnouncementType Type { get; set; }

        [Required(ErrorMessage = "Description Required")]
        [MaxLength(300)]
        public string Description { get; set; }

        public AnnouncementImage Image { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "User Author Required")]
        public User User { get; set; }
    }
}
