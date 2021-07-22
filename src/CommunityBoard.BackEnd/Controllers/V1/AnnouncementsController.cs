using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.BackEnd.Utilities.Helpers;
using CommunityBoard.Core.Contracts.Requests.Queries;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Interfaces.Services;
using CommunityBoard.Core.Models.CoreModels;
using CommunityBoard.Core.Models.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public AnnouncementsController(
            IAnnouncementsRepository announcementRepository, 
            IMapper mapper, 
            IUriService uriService)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
            _uriService = uriService;
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

            var locationUri = _uriService.GetAnnouncementUri(announcement.Id.ToString());
            
            //201 - Created
            return Created(locationUri, _mapper.Map<Announcement, AnnouncementResponse>(announcement));
        }


        [AllowAnonymous] //Anyone can view all announcements
        [HttpGet(ApiRoutes.Announcements.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var announcements = await _announcementRepository.FindAllAsync(pagination);
            var announcementResponse = _mapper.Map<List<AnnouncementResponse>>(announcements);

            if (pagination == null || 
                pagination.PageNumber < 1 || 
                pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<AnnouncementResponse>(announcementResponse));
            }

            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, announcementResponse);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Announcements.Get)]
        public async Task<IActionResult> Get([FromRoute] int announcementId)
        {
            var announcement = await _announcementRepository.FindByIdAsync(announcementId);
            if (announcement == null)
                return NotFound(new { Error = "Announcement was not found." });
            return Ok(new Response<AnnouncementResponse>(
                _mapper.Map<AnnouncementResponse>(announcement))
            );
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Announcements.GetByName)]
        public async Task<IActionResult> GetByName([FromRoute] string announcementName)
        {
            var announcement = 
                await _announcementRepository.FindAnnouncementsByName(announcementName);
            return Ok(new Response<AnnouncementResponse>(
                _mapper.Map<AnnouncementResponse>(announcement))
            );
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
                return Ok(_mapper.Map<AnnouncementResponse>(announcement));

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

            return Ok(new Response<List<AnnouncementResponse>>( 
                _mapper.Map<List<AnnouncementResponse>>(announcements))
            );
        }
    }
}