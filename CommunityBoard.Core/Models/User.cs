using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityBoard.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name Required")]
        [MaxLength(500)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name Required")]
        [MaxLength(500)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username Required")]
        [MaxLength(500)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [MaxLength(500)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 charachters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [MaxLength(350)]
        public string Email { get; set; }

        [Display(Name = "Date of Registration")]
        [Required(ErrorMessage = "Date Required")]
        public DateTime DateRegistered { get; set; }

        [Display(Name = "Number Of Reports")]
        public int? NumberOfReports { get; set; }

        public IList<Announcement> Announcements { get; set; }
    }
}