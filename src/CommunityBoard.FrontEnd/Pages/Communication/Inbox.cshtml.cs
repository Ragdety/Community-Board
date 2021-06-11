using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CommunityBoard.FrontEnd.Pages.Communication
{
    public class InboxModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IChatClient _chatClient;
        private readonly ILogger<InboxModel> _logger;

        public InboxModel(
            IConfiguration configuration,
            IChatClient chatClient,
            ILogger<InboxModel> logger)
        {
            _configuration = configuration;
            _chatClient = chatClient;
            _logger = logger;
            ChatUrl = _configuration["serviceUrl"] + _configuration["chatHub"];
        }

        public Message Message { get; set; }
        public string ChatUrl { get; }
        public List<Chat> Chats { get; set; }
        
        
        public async Task<IActionResult> OnGet()
        {
            try
            {
                Chats = await _chatClient.GetUserChats();
                if (Chats == null)
                    return RedirectToPage("/Index");
                return Page();
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message);
                ModelState.AddModelError("Error", "Something went wrong...");
                return Page();
            }
        }
    }
}