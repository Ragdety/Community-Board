using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Clients;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Services.V1
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

		public async Task<UserDto> GetUserById(int userId)
		{
            var response = await _httpClient.GetAsync(
                ApiRoutes.Identity.GetUser.Replace("{userId}", userId.ToString()));

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            return await response.Content.ReadAsAsync<UserDto>();
		}

		public async Task<AuthenticationResult> Login(UserLoginDto user)
        {
            var loginResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Login, user);

            if(!loginResponse.IsSuccessStatusCode)
            {
                return new AuthenticationResult
                {
                    Token = null,
                    RefreshToken = null,
                    Success = false,
                    Errors = new[] { "Invalid username/email or password" }
                };
            }

            loginResponse.EnsureSuccessStatusCode();
            var response = await loginResponse.Content.ReadAsAsync<AuthSuccessResponse>();

            return new AuthenticationResult
            {
                Token = response.Token,
                RefreshToken = response.RefreshToken,
                Success = true,
                Errors = null
            };
        }

        public async Task<AuthenticationResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResult> Register(UserRegistrationDto user)
        {
            var registrationResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Register, user);

            if (!registrationResponse.IsSuccessStatusCode)
            {
                var failedResponse = await registrationResponse.Content.ReadAsAsync<AuthFailedResponse>();
                return new AuthenticationResult
                {
                    Token = null,
                    RefreshToken = null,
                    Success = false,
                    Errors = failedResponse.Errors
                };
            }

            registrationResponse.EnsureSuccessStatusCode();
            var response = await registrationResponse.Content.ReadAsAsync<AuthSuccessResponse>();

            return new AuthenticationResult
            {
                Token = response.Token,
                RefreshToken = response.RefreshToken,
                Success = true,
                Errors = null
            };
        }
    }
}
