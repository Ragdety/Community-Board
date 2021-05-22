using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Controllers.V1.CommunicationControllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _announcementRepository;

        public AnnouncementsController(IAnnouncementsRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        [HttpPost(ApiRoutes.Announcements.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto announcementDto)
        {
            int userId;
            try
            {
                userId = HttpContext.GetUserId();
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }

            var announcement = new Announcement
            {
                Name = announcementDto.Name,
                Type = (AnnouncementType)Enum.Parse(typeof(AnnouncementType), announcementDto.Type),
                Description = announcementDto.Description,
                Image = announcementDto.Image,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            await _announcementRepository.CreateAsync(announcement);

            //https://localhost:5001 in this case
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + ApiRoutes.Announcements.Get.Replace("{id}", announcement.Id.ToString());
            var response = new AnnouncementResponse { Id = announcement.Id };

            //201 - Created
            return Created(locationUri, response);
        }


        [AllowAnonymous] //Anyone can view all announcements
        [HttpGet(ApiRoutes.Announcements.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _announcementRepository.FindAllAsync());
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Announcements.Get)]
        public async Task<IActionResult> Get([FromRoute] int announcementId)
        {
            var announcement = await _announcementRepository.FindByIdAsync(announcementId);
            if (announcement == null)
                return NotFound(new { Error = "Announcement was not found." });
            return Ok(announcement);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Announcements.GetByName)]
        public async Task<IActionResult> GetByName([FromRoute] string announcementName)
		{
            return Ok(await _announcementRepository.FindAnnouncementsByName(announcementName));
		}

        [HttpPut(ApiRoutes.Announcements.Update)]
        public async Task<IActionResult> Update(
            [FromRoute] int announcementId,
            [FromBody] UpdateAnnouncementDto announcementDto)
        {
            var userOwnsAnnouncement 
                = await _announcementRepository.UserOwnsAnnouncementAsync(
                    announcementId, HttpContext.GetUserId());

            if (!userOwnsAnnouncement)
                return BadRequest(new { Error = "You do not own this announcement" });

            var announcement = await _announcementRepository.FindByIdAsync(announcementId);
            announcement.Name = announcementDto.Name;
            announcement.Type = (AnnouncementType)Enum.Parse(typeof(AnnouncementType), announcementDto.Type);
            announcement.Description = announcementDto.Description;
            announcement.Image = announcementDto.Image;

            var updated = await _announcementRepository.UpdateAsync(announcement);
            if (updated)
                return Ok(announcement);

            return NotFound(new { Error = "Announcement was not found." });
        }

        [HttpDelete(ApiRoutes.Announcements.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int announcementId)
        {
            var userOwnsAnnouncement
                = await _announcementRepository.UserOwnsAnnouncementAsync(
                    announcementId, HttpContext.GetUserId());

            if (!userOwnsAnnouncement)
                return BadRequest(new { Error = "You do not own this announcement" });

            var announcementToDelete = await _announcementRepository.FindByIdAsync(announcementId);
            
            var deleted = await _announcementRepository.DeleteAsync(announcementToDelete);
            if (deleted)
                return NoContent();

            return NotFound(new { Error = "Announcement was not found." });
        }

        //Extra
        [HttpGet(ApiRoutes.Announcements.GetFromUser)]
        public async Task<IActionResult> GetAnnouncementsFromUser()
        {
            int userId;
            try
            {
                userId = HttpContext.GetUserId();
            }
            catch(ArgumentNullException)
            {
                return Unauthorized(new { ErrorMessage = "You're not allowed" });
            }

            var announcements = await _announcementRepository.FindAllUserAnnouncements(userId);
            if (announcements == null)
                return NotFound();

            return Ok(announcements);
        }
    }
}