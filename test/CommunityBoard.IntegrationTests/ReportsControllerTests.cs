using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Models;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CommunityBoard.IntegrationTests
{
	public class ReportsControllerTests : IntegrationTest
	{
		[Fact]
		public async Task Create_ReturnsOk_WhenCreated()
		{
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            var createdAnnouncement = new CreateAnnouncementDto
            {
                Name = "Announcement To Report",
                Type = "Other",
                Description = "Report",
                Image = null
            };

            var announcementResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, createdAnnouncement);
            var announcementContent = await announcementResponse.Content.ReadAsAsync<AnnouncementResponse>();

            //Act
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementContent.Id.ToString()), new CreateReportDto
                    {
                        ReportCause = "Test",
                        ReportDescription = "Test",
                    });
            var content = await response.Content.ReadAsAsync<ReportResponse>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            content.Should().NotBeNull();
            content.Should().BeOfType<ReportResponse>();
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenAdminLoggedIn()
        {
            //Arrange
            await LoginAsAdminAsync();
            var createdAnnouncement = new CreateAnnouncementDto
            {
                Name = "Announcement To Report",
                Type = "Other",
                Description = "Report",
                Image = null
            };

            var announcementResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, createdAnnouncement);
            var announcementContent = await announcementResponse.Content.ReadAsAsync<AnnouncementResponse>();
            int announcementId = announcementContent.Id;

            var createdReport = new CreateReportDto
            {
                ReportCause = "Test",
                ReportDescription = "Test",
            };

            var reportResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementId.ToString()), createdReport);

            var reportResponseContent = 
                await reportResponse.Content.ReadAsAsync<ReportResponse>();

            //Act
            var response = await TestClient.GetAsync(
                ApiRoutes.Reports.Get.Replace(
                    "{reportId}", reportResponseContent.Id.ToString()));
            var content = await response.Content.ReadAsAsync<Report>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            content.Should().NotBeNull();
            content.ReportCause.Should().Be("Test");
            content.ReportDescription.Should().Be("Test");
        }

        [Fact]
        public async Task Get_ReturnsForbidden_WhenUserLoggedIn()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();
            var announcementResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Announcement To Report",
                    Type = "Other",
                    Description = "Report",
                    Image = null
                });
            var announcementContent = await announcementResponse.Content.ReadAsAsync<AnnouncementResponse>();

            var reportResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementContent.Id.ToString()), new CreateReportDto
                    {
                        ReportCause = "Test",
                        ReportDescription = "Test",
                    });

            var reportResponseContent =
                await reportResponse.Content.ReadAsAsync<ReportResponse>();

            //Act
            var response = await TestClient.GetAsync(
                ApiRoutes.Reports.Get.Replace(
                    "{reportId}", reportResponseContent.Id.ToString()));

            var content = await response.Content.ReadAsAsync<Report>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            content.Should().BeNull();
        }

        [Fact]
        public async Task GetAllFromAnnouncement_ReturnsOk_WhenAdminLoggedIn()
        {
            //Arrange
            await LoginAsAdminAsync();
            var announcementResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Announcement To Report",
                    Type = "Other",
                    Description = "Report",
                    Image = null
                });
            var announcementContent = await announcementResponse.Content.ReadAsAsync<AnnouncementResponse>();

            await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementContent.Id.ToString()), new CreateReportDto
                    {
                        ReportCause = "Test",
                        ReportDescription = "Test",
                    });

            await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementContent.Id.ToString()), new CreateReportDto
                    {
                        ReportCause = "Test",
                        ReportDescription = "Test",
                    });
            //Act
            var response = await TestClient.GetAsync(
                ApiRoutes.Reports.GetAllFromAnnouncement.Replace(
                    "{announcementId}", announcementContent.Id.ToString()));

            var content = await response.Content.ReadAsAsync<List<Report>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNullOrEmpty();
            content.Should().ContainItemsAssignableTo<Report>();
            content.Should().NotContainNulls();
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenAdminLoggedIn()
        {
            //Arrange
            await LoginAsAdminAsync();
            var announcementResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Announcement To Report",
                    Type = "Other",
                    Description = "Report",
                    Image = null
                });
            var announcementContent = await announcementResponse.Content.ReadAsAsync<AnnouncementResponse>();

            var createResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementContent.Id.ToString()), new CreateReportDto
                    {
                        ReportCause = "Test",
                        ReportDescription = "Test",
                    });

            var createReportContent = createResponse.Content.ReadAsAsync<ReportResponse>();

            //Act
            var response = await TestClient.DeleteAsync(
                ApiRoutes.Reports.Delete.Replace(
                    "{reportId}", createReportContent.Id.ToString()));

            var content = await response.Content.ReadAsAsync<List<Report>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            content.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Delete_ReturnsForbidden_WhenUserLoggedIn()
        {
            //Arrange
            await AuthenticateAsync();
            await LoginAsync();

            var announcementResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Announcements.Create, new CreateAnnouncementDto
                {
                    Name = "Announcement To Report",
                    Type = "Other",
                    Description = "Report",
                    Image = null
                });
            var announcementContent = await announcementResponse.Content.ReadAsAsync<AnnouncementResponse>();

            var createResponse = await TestClient.PostAsJsonAsync(
                ApiRoutes.Reports.Create.Replace("{announcementId}",
                    announcementContent.Id.ToString()), new CreateReportDto
                    {
                        ReportCause = "Test",
                        ReportDescription = "Test",
                    });

            var createReportContent = createResponse.Content.ReadAsAsync<ReportResponse>();

            //Act
            var response = await TestClient.DeleteAsync(
                ApiRoutes.Reports.Delete.Replace(
                    "{reportId}", createReportContent.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}