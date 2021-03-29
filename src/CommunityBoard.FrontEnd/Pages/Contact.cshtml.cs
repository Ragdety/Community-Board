using System.Threading.Tasks;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IAnnouncementClient _apiAnnouncementClient;

        [BindProperty]
        public string EmailSubject { get; set; }

        [BindProperty]
        public string EmailBody { get; set; }

		public Announcement Announcement { get; set; }

		public ContactModel(IAnnouncementClient apiAnnouncementClient)
		{
			_apiAnnouncementClient = apiAnnouncementClient;
		}

		public async Task<IActionResult> OnGet(int announcementId)
        {
            Announcement = 
                await _apiAnnouncementClient.GetAnnouncementByIdAsync(announcementId);

            if (Announcement == null)
                return NotFound(new { Message = "Announcement was not found" });

            return Page();
        }
    }
}