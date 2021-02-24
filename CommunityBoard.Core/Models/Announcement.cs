using CommunityBoard.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "nvarchar(50)")]
        public AnnouncementType Type { get; set; }

        [Required(ErrorMessage = "Description Required")]
        [MaxLength(300)]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "User Author Id Required")]
        [ForeignKey("UserFK")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
