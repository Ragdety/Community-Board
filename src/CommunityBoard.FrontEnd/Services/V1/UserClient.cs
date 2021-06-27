using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using Microsoft.AspNetCore.Http;

namespace CommunityBoard.FrontEnd.Services.V1
{
    public class UserClient : BaseClient, IUserClient
    {
        public UserClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {
        }
        
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync(
                ApiRoutes.Users.GetUserById.Replace("{userId}", userId.ToString()));

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            return await response.Content.ReadAsAsync<UserDto>();
        }
    }
}