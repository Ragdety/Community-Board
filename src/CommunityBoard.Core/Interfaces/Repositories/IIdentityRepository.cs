using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.DTOs;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IIdentityRepository
    {
        Task<AuthenticationResult> RegisterAsync(
            string firstName, string lastName, string userName, string email, string password);
        Task<AuthenticationResult> LoginAsync(string emailOrUserName, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<UserDto> FindUserById(int userId);
    }
}