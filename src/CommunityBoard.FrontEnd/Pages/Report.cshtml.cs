using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Pages
{
    public class ReportModel : PageModel
    {
        private readonly IReportClient _reportClient;

		public ReportModel(IReportClient reportClient)
		{
			_reportClient = reportClient;
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

            if(!success)
			{
                ModelState.AddModelError("Error", "An unknown error occured, try again later.");
                return Page();
			}

            return RedirectToPage("/Index");
		}
    }
}