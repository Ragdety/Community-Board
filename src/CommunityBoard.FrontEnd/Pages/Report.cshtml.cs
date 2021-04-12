using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Pages
{
    public class ReportModel : PageModel
    {
        private readonly IReportClient _reportClient;
        private readonly IFluentEmail _emailSender;

		public ReportModel(IReportClient reportClient, IFluentEmail emailSender)
		{
			_reportClient = reportClient;
			_emailSender = emailSender;
		}

		[BindProperty]
        public string ReportCause { get; set; }

        [BindProperty]
        public string ReportDescription { get; set; }

        [BindProperty]
		public int AnnouncementId { get; set; }

		public void OnGet(int announcementId)
        {
            AnnouncementId = announcementId;
        }

        public async Task<IActionResult> OnPostAsync()
		{
			var success = await _reportClient.CreateReportAsync(AnnouncementId, new CreateReportDto
            { 
                ReportCause = ReportCause,
                ReportDescription = ReportDescription
            });

            //Send email to admin
            var emailResponse = await _emailSender
                .To("cis1512.communityboard@gmail.com")
                .Subject("New Announcement Reported")
                .Body($"Cause: {ReportCause}")
                .SendAsync();

            if (!success || !emailResponse.Successful)
			{
                ModelState.AddModelError("Error", "An unknown error occured, try again later.");
                return Page();
			}

            return RedirectToPage("/Index");
		}
    }
}