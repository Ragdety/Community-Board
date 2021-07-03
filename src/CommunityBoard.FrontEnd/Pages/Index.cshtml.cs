using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.CommunicationDTOs;
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
        private readonly IChatClient _chatClient;

        public IndexModel(
            ILogger<IndexModel> logger, 
            IAnnouncementClient apiAnnouncementClient,
            IChatClient chatClient)
		{
			_logger = logger;
			_apiAnnouncementClient = apiAnnouncementClient;
            _chatClient = chatClient;
        }

		public IEnumerable<Announcement> Announcements { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        
        [BindProperty] 
        public string Message { get; set; }

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

        public async Task<IActionResult> OnPostSendMessage(int userId)
        {
            var chatResponse = await _chatClient.CreateUserChatAsync(userId);
            var successMessage = 
                await _chatClient.SendMessageAsync(chatResponse.Id, new CreateMessageDto 
                {
                    Text = Message
                });

            //Will append the chat id in Url here later:
            if (successMessage) return RedirectToPage("/Communication/Inbox");
            ModelState.AddModelError("SendMessageError", "Error sending message. Try again later...");
            return Page();
        }
        

        public string RetrieveImage(byte[] image)
        {
            string imageBase64Data = Convert.ToBase64String(image);
            string imageDataUrl =
                $"data:image/png;base64,{imageBase64Data}";

            return imageDataUrl;
        }

        public bool IsAnnouncementMine(int announcementUserId)
        {
            if (HttpContext.User == null) return false;
            string userId = HttpContext.User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId)) return false;
            return Int32.Parse(userId) == announcementUserId;
        }
    }
}