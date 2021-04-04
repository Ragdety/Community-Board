using CommunityBoard.BackEnd;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

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
                    builder.ConfigureServices(async services =>
                    {
                        //https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(descriptor);
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            //Using this for testing purposes
                            options.UseInMemoryDatabase("TestDb");
                        });

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                            var logger = scopedServices
                                .GetRequiredService<ILogger<WebApplicationFactory<Startup>>>();

                            db.Database.EnsureCreated();

                            var roleManager =
                                scopedServices.GetRequiredService<RoleManager<IdentityRole<int>>>();

                            await AddRolesAsync(roleManager);
                        }
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
                    FirstName = "UserTest",
                    LastName = "UserTest",
                    UserName = "UserTest",
                    Email = "UserTest@integrationTests.com",
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
                    EmailOrUserName = "UserTest",
                    Password = "Test1234",
                });

            var registraionResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registraionResponse.Token;
        }

        private async Task AddRolesAsync(RoleManager<IdentityRole<int>> roleManager)
		{
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole<int>("Admin");
                await roleManager.CreateAsync(adminRole);
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                var userRole = new IdentityRole<int>("User");
                await roleManager.CreateAsync(userRole);
            }
        }
    }
}