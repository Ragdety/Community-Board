using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CommunityBoard.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAnnouncementClient _apiAnnouncementClient;

        public IndexModel(ILogger<IndexModel> logger, IAnnouncementClient apiAnnouncementClient)
        {
            _logger = logger;
            _apiAnnouncementClient = apiAnnouncementClient;
        }

        public IEnumerable<Announcement> Announcements;

        public async Task OnGet()
        {
            Announcements = await _apiAnnouncementClient.GetAnnouncementsAsync();
        }
    }
}