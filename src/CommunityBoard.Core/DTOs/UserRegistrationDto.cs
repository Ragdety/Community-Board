using System;

namespace CommunityBoard.Core.DTOs
{
    public class UserRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
