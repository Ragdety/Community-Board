using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommunityBoard.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(500)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Username { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Password { get; set; }

        [Required]
        [MaxLength(3000)]
        public string Email { get; set; }

        [Required]
        public DateTime DateRegistered { get; set; }
    }
}
