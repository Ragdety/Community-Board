using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Pages.PostManagement
{
	public class ManageModel : PageModel
    {
        private readonly IAnnouncementClient _apiAnnouncementClient;

		public ManageModel(IAnnouncementClient apiAnnouncementClient)
        {
            _apiAnnouncementClient = apiAnnouncementClient;
        }

        public Tuple<List<Announcement>, string> UserAnnouncements;

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = (ClaimsIdentity)HttpContext.User.Identity;
                string id = user.FindFirst("id")?.Value;
                var token = Request.Cookies["JWToken"];

                //For now add it from cookie
                //If time allows it, will add security to prevent attacks
                UserAnnouncements =
                   await _apiAnnouncementClient.GetUserAnnouncementsAsync(int.Parse(id), token);
            }
        }

        //Delete
        public async Task<IActionResult> OnPost(int announcementId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = Request.Cookies["JWToken"];
                await _apiAnnouncementClient.DeleteAnnouncementAsync(announcementId, token);
                return RedirectToPage("/PostManagement/Manage");
            }
            return RedirectToPage("/Index");
        }
    }
}