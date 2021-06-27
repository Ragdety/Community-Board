using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> FindUserByIdAsync(int userId);
        Task<UserDto> FindUserByUsernameAsync(string username);
    }
}