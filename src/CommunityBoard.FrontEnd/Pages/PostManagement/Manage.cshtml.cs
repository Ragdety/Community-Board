using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
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

        public IEnumerable<Announcement> Announcements;

        public async Task OnGet()
        {
            //For now:
            Announcements = await _apiAnnouncementClient.GetAnnouncementsAsync();
        }
    }
}