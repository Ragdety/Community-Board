using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1
{
	[Authorize(AuthenticationSchemes = 
        JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IMapper _mapper;

        public ReportsController(
            IReportsRepository reportsRepository,
            IMapper mapper)
        {
            _reportsRepository = reportsRepository;
            _mapper = mapper;
        }

		[HttpGet(ApiRoutes.Reports.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _reportsRepository.FindAllAsync(); 
            return Ok(_mapper.Map<List<Report>>(reports));
		}

        [HttpGet(ApiRoutes.Reports.Get)]
        public async Task<IActionResult> Get([FromRoute] int reportId)
        {
            var report = await _reportsRepository.FindByIdAsync(reportId);

            if (report == null)
                return NotFound(new { Error = "Report was not found" });

            return Ok(_mapper.Map<Report>(report));
        }

        [HttpGet(ApiRoutes.Reports.GetAllFromAnnouncement)]
        public async Task<IActionResult> GetAllFromAnnouncement([FromRoute] int announcementId)
        {
            var reports = await _reportsRepository.FindAllReportsFromAnnouncement(announcementId);
            return Ok(_mapper.Map<List<Report>>(reports));
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Reports.Create)]
        public async Task<IActionResult> Create(
            [FromRoute] int announcementId,
            [FromBody] CreateReportDto reportDto)
        {
			var report = new Report
			{
                ReportCause = reportDto.ReportCause,
                ReportDescription = reportDto.ReportDescription,
                ReportDate = DateTime.UtcNow,
                AnnouncementId = announcementId
            };

            await _reportsRepository.CreateAsync(report);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + ApiRoutes.Reports.Get.Replace("{reportId}", report.Id.ToString());
            return Created(locationUri, _mapper.Map<Report>(report));
        }

        [HttpDelete(ApiRoutes.Reports.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int reportId)
        {
            var reportToDelete = await _reportsRepository.FindByIdAsync(reportId);
            
            var deleted = await _reportsRepository.DeleteAsync(reportToDelete);
            if (deleted)
                return NoContent();

            return NotFound(new { Error = "Report was not found." });
        }

        [HttpPut(ApiRoutes.Reports.Update)]
        public Task<IActionResult> Update([FromBody] CreateReportDto reportDto)
        {
            throw new NotImplementedException();
        }
    }
}