using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Controllers.V1
{
	[Authorize(AuthenticationSchemes = 
        JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportsRepository _reportsRepository;

		public ReportsController(IReportsRepository reportsRepository)
		{
			_reportsRepository = reportsRepository;
		}

		[HttpGet(ApiRoutes.Reports.GetAll)]
        public async Task<IActionResult> GetAll()
		{
            return Ok(await _reportsRepository.FindAllAsync());
		}

        [HttpGet(ApiRoutes.Reports.Get)]
        public async Task<IActionResult> Get([FromRoute] int reportId)
        {
            var report = await _reportsRepository.FindByIdAsync(reportId);

            if (report == null)
                return NotFound(new { Error = "Report was not found" });

            return Ok(report);
        }

        [HttpGet(ApiRoutes.Reports.GetAllFromAnnouncement)]
        public async Task<IActionResult> GetAllFromAnnouncement([FromRoute] int announcementId)
        {
            return Ok(await _reportsRepository.FindAllReportsFromAnnouncement(announcementId));
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
            var response = new ReportResponse { Id = report.Id };

            return Created(locationUri, response);
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

        //For now update not available
    }
}