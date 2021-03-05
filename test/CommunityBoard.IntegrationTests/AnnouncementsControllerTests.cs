using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
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
    }
}