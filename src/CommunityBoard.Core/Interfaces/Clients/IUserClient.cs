using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;

namespace CommunityBoard.Core.Interfaces.Clients
{
    public interface IUserClient
    {
        Task<UserDto> GetUserByIdAsync(int userId);
    }
}