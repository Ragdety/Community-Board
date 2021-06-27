using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Identity;

namespace CommunityBoard.BackEnd.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<UserDto> FindUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user == null ? null : MapUser(user);
        }
        
        public async Task<UserDto> FindUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user == null ? null : MapUser(user);
        }
        
        private static UserDto MapUser(User user)
        {
            var userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };

            return userDto;
        }
    }
}