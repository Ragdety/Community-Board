using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CommunityBoard.IntegrationTests
{
    public class AnnouncementsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithAnnouncements_ReturnsList()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Announcements.GetAll);
            var content = await response.Content.ReadAsAsync<List<Announcement>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsAnnouncement_WhenExist()
        {
            //Arrange
            await LoginAsync();
            var createdAnnouncement = await CreateAnnouncementAsync(new CreateAnnouncementDto
            {
                Name = "Test Announcement",
                Type = "Sale",
                Description = "Test Description",
                Image = null
            });

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Announcements.Get.Replace(
                    "{announcementId}", createdAnnouncement.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedAnnouncement = await response.Content.ReadAsAsync<Announcement>();
            returnedAnnouncement.Id.Should().Be(createdAnnouncement.Id);
            returnedAnnouncement.Name.Should().Be("Test Announcement");
            returnedAnnouncement.Type.Should().Be(AnnouncementType.Sale);
            returnedAnnouncement.Description.Should().Be("Test Description");
            returnedAnnouncement.Image.Should().BeNull();
        }

        [Fact]
        public async Task Create_ReturnsOk_WhenCreatedSuccessfully()
		{
            //Arrange
            await AuthenticateAsync();
            await LoginAsync(); //Adds token
            var createdAnnouncement = new CreateAnnouncementDto
            {
                Name = "Create Integration Test",
                Type = "Sale",
                Description = "Create Integration Test Description",
                Image = null
            };

            //Act
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, createdAnnouncement);
            var content = await response.Content.ReadAsAsync<AnnouncementResponse>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeNull();
            content.Should().BeOfType<AnnouncementResponse>();
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenNotLoggedIn()
        {
            //Arrange
            var createdAnnouncement = new CreateAnnouncementDto
            {
                Name = "Not logged in test",
                Type = "Other",
                Description = "Not logged in test description",
                Image = null
            };

            //Act
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, createdAnnouncement);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //Arrange


        //Act


        //Assert

    }
}