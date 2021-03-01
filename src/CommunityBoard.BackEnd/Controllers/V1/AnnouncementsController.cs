using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _announcementRepository;
        private readonly IIdentityRepository _identityRepository;

        public AnnouncementsController(
            IAnnouncementsRepository announcementRepository, 
            IIdentityRepository identityRepository)
        {
            _announcementRepository = announcementRepository;
            _identityRepository = identityRepository;
        }

        [AllowAnonymous] //Everyone can view all announcements
        [HttpGet(ApiRoutes.Announcements.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _announcementRepository.FindAllAsync());
        }

        [HttpPost(ApiRoutes.Announcements.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto announcementDto)
        {
            int userId;
            try
            {
                userId = HttpContext.GetUserId();
            }
            catch(ArgumentNullException)
            {
                return Unauthorized();
            }

            var announcement = new Announcement
            {
                Name = announcementDto.Name,
                Type = announcementDto.Type,
                Description = announcementDto.Description,
                Image = announcementDto.Image,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            await _announcementRepository.CreateAsync(announcement);

            //https://localhost:5001 in this case
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var locationUri =  baseUrl + ApiRoutes.Announcements.Get.Replace("{id}", announcement.Id.ToString());
            var response = new AnnouncementResponse { Id = announcement.Id };

            //201 - Created
            return Created(locationUri, response);
        }
    }
}