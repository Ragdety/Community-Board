using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.DTOs;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Clients
{
    public interface IIdentityClient
    {
        Task<AuthenticationResult> Register(UserRegistrationDto user);
        Task<AuthenticationResult> Login(UserLoginDto user);
        Task<UserDto> GetUserById(int userId);
        Task<AuthenticationResult> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}