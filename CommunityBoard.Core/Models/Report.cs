using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommunityBoard.Core.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReportCause { get; set; }

        [Required]
        [MaxLength(300)]
        public string ReportDescription { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        [Required]
        public int AnnouncementId { get; set; }
        [ForeignKey(nameof(AnnouncementId))]
        public virtual Announcement Announcement { get; set; }
    }
}
