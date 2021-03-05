using CommunityBoard.BackEnd;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace CommunityBoard.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(ApplicationDbContext));
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            //Use this for testing purposes
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task LoginAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetJwtLoginAsync());
        }

        protected async Task<AnnouncementResponse> CreateAnnouncementAsync(
            CreateAnnouncementDto createAnnouncementDto)
        {
            var res = await TestClient.PostAsJsonAsync(ApiRoutes.Announcements.Create, createAnnouncementDto);
            return await res.Content.ReadAsAsync<AnnouncementResponse>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.Identity.Register, new UserRegistrationDto
                {
                    FirstName = "JohnTest",
                    LastName = "DoeTest",
                    UserName = "JohnDoeTest",
                    Email = "test@integration.com",
                    Password = "Test1234",
                    DateRegistered = DateTime.UtcNow
                });

            var registraionResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registraionResponse.Token;
        }

        private async Task<string> GetJwtLoginAsync()
        {
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.Identity.Login, new UserLoginDto
                {
                    EmailOrUserName = "JohnDoeTest",
                    Password = "Test1234",
                });

            var registraionResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registraionResponse.Token;
        }
    }
}