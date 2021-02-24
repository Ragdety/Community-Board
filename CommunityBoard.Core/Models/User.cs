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
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name Required")]
        [MaxLength(500)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [MaxLength(350)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public DateTime DateRegistered { get; set; }
    }
}