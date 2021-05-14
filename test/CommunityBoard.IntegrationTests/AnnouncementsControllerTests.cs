using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Models.CoreModels;
using FluentAssertions;
using Newtonsoft.Json;
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
        public async Task Update_ReturnsOk_WhenUpdatedSuccessfully()
		{
			//Arrange
			await AuthenticateAsync();
			await LoginAsync();
            var createResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });

            var announcementResponse = await createResponse.Content.ReadAsAsync<AnnouncementResponse>();

            //Act
            var response = await TestClient.PutAsJsonAsync(
                ApiRoutes.Announcements.Update.Replace(
                    "{announcementId}", announcementResponse.Id.ToString()), 
                new UpdateAnnouncementDto
                {
                    Name = "Integration Test Update",
                    Description = "Integration Test Update Description",
                    Type = "Other",
                    Image = null
                });
            var returnedUpdatedAnnouncement = await response.Content.ReadAsAsync<Announcement>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            returnedUpdatedAnnouncement.Should().NotBeNull();
            returnedUpdatedAnnouncement.Name.Should().Be("Integration Test Update");
            returnedUpdatedAnnouncement.Description.Should().Be("Integration Test Update Description");
            returnedUpdatedAnnouncement.Type.Should().Be(AnnouncementType.Other);
            returnedUpdatedAnnouncement.Image.Should().BeNull();
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenWrongIdPassed()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });

            //Act
            var response = await TestClient.PutAsJsonAsync(
                ApiRoutes.Announcements.Update, new UpdateAnnouncementDto
                {
                    Name = "Wrong Integration Test Update",
                    Description = "Wrong Integration Test Update Description",
                    Type = "Other",
                    Image = null
                });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            var postResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });
            var announcementResponse = await postResponse.Content.ReadAsAsync<AnnouncementResponse>();

            //Act
            var response = await TestClient.DeleteAsync(
                ApiRoutes.Announcements.Delete.Replace(
                    "{announcementId}", announcementResponse.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            content.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenUserDoesNotOwnIt()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });

            //Act
            var response = await TestClient.DeleteAsync(
                ApiRoutes.Announcements.Delete.Replace(
                    "{announcementId}", "99999"));
            string content = await response.Content.ReadAsStringAsync();
            dynamic errorResponse = JsonConvert.DeserializeObject<dynamic>(content);
            string error = errorResponse.error;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            error.Should().NotBeNullOrEmpty();
            error.Should().BeEquivalentTo("You do not own this announcement");
        }

        [Fact]
        public async Task GetFromUser_ReturnsOk_WhenUserIsAuthorized()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test 2",
                    Description = "Test 2",
                    Type = "Sale",
                    Image = null
                });

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Announcements.GetFromUser);
            //^User Id will be detected by the Http Context
            var content = await response.Content.ReadAsAsync<List<Announcement>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetFromUser_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test 2",
                    Description = "Test 2",
                    Type = "Sale",
                    Image = null
                });

            //Act
            Logout();
            var response = await TestClient.GetAsync(ApiRoutes.Announcements.GetFromUser);
            //^User Id will be detected by the Http Context
            var content = await response.Content.ReadAsAsync<List<Announcement>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            content.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetByName_ReturnsOk_WhenExists()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });

            //Act
            var response = await TestClient.GetAsync(
                ApiRoutes.Announcements.GetByName.Replace(
                    "{announcementName}", "Test"));
            //^User Id will be detected by the Http Context
            var content = await response.Content.ReadAsAsync<List<Announcement>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNullOrEmpty();
            content[0].Name.Should().Be("Test");
            content[0].Description.Should().Be("Test");
            content[0].Type.Should().Be(AnnouncementType.Club);
            content[0].Image.Should().BeNull();
        }

        [Fact]
        public async Task GetByName_ReturnsOkButEmptyResponse_WhenDoesNotExist()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Test",
                    Description = "Test",
                    Type = "Club",
                    Image = null
                });

            //Act
            var response = await TestClient.GetAsync(
                ApiRoutes.Announcements.GetByName.Replace(
                    "{announcementName}", "RandomNameHere"));
            //^User Id will be detected by the Http Context
            var content = await response.Content.ReadAsAsync<List<Announcement>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().BeNullOrEmpty();
        }
    }
}