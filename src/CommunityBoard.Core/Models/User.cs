using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityBoard.Core.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(500)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(500)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateRegistered { get; set; }
    }
}