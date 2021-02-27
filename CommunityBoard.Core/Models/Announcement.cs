using CommunityBoard.Core.Enums;
using Microsoft.AspNetCore.Identity;
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

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public AnnouncementType Type { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser User { get; set; }
    }
}
