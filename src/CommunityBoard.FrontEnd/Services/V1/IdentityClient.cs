using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Clients;
using Microsoft.AspNetCore.Http;
using System;
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

        public async Task<AuthenticationResult> Login(UserLoginDto user)
        {
            var loginResultResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Login, user);

            if(!loginResultResponse.IsSuccessStatusCode)
            {
                return new AuthenticationResult
                {
                    Token = null,
                    RefreshToken = null,
                    Success = false,
                    Errors = new[] { "Invalid username/email or password" }
                };
            }

            loginResultResponse.EnsureSuccessStatusCode();
            var response = await loginResultResponse.Content.ReadAsAsync<AuthSuccessResponse>();

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
            throw new NotImplementedException();
        }
    }
}
