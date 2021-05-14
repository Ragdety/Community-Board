using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CommunityBoard.FrontEnd.Pages
{
	public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAnnouncementClient _apiAnnouncementClient;

		public IndexModel(
            ILogger<IndexModel> logger, 
            IAnnouncementClient apiAnnouncementClient)
		{
			_logger = logger;
			_apiAnnouncementClient = apiAnnouncementClient;
		}

		public IEnumerable<Announcement> Announcements { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SearchTerm))
                {
                    Announcements = await _apiAnnouncementClient.GetAnnouncementsAsync();
                }
                else
                {
                    Announcements = await _apiAnnouncementClient
                        .GetAnnouncementsByNameAsync(SearchTerm.Trim());
                }
            }
            catch { }
        }

        public string RetrieveImage(byte[] image)
        {
            string imageBase64Data = Convert.ToBase64String(image);
            string imageDataURL =
                string.Format("data:image/png;base64,{0}", imageBase64Data);

            return imageDataURL;
        }
    }
}