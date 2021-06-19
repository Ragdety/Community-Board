using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CoreModels;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages
{
	public class ContactModel : PageModel
    {
        private readonly IAnnouncementClient _apiAnnouncementClient;
        private readonly IIdentityClient _identityClient;
        private readonly IFluentEmail _emailSender;

        [BindProperty]
        public string EmailSubject { get; set; }

        [BindProperty]
        public string EmailBody { get; set; }

		public Announcement Announcement { get; set; }

        [BindProperty]
		public int AnnouncementId { get; set; }

        [BindProperty]
		public UserDto AnnouncementUser { get; set; }

		public ContactModel(
            IAnnouncementClient apiAnnouncementClient, 
            IIdentityClient identityClient,
            IFluentEmail emailSender)
		{
			_apiAnnouncementClient = apiAnnouncementClient;
			_identityClient = identityClient;
            _emailSender = emailSender;
        }

		public async Task<IActionResult> OnGetAsync(int announcementId)
        {
            AnnouncementId = announcementId;
            Announcement = 
                await _apiAnnouncementClient.GetAnnouncementByIdAsync(announcementId);

            AnnouncementUser = await _identityClient.GetUserByIdAsync(Announcement.UserId);

            if (Announcement == null)
                return RedirectToPage("/Errors/NotFound");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
		{
            string userEmail = 
                HttpContext.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            Announcement =
                await _apiAnnouncementClient.GetAnnouncementByIdAsync(AnnouncementId);

            AnnouncementUser =
                await _identityClient.GetUserByIdAsync(Announcement.UserId);

			//Might add a template
			var emailResponse = await _emailSender
				.SetFrom(userEmail)
				.To(AnnouncementUser.Email, AnnouncementUser.FirstName)
				.Subject(EmailSubject)
				.Body(EmailBody)
				.SendAsync();

			if (!emailResponse.Successful)
			{
				ModelState.AddModelError("EmailError", "Email could not be sent, try again later.");
				return Page();
			}

			return RedirectToPage("/Index");
		}
    }
}