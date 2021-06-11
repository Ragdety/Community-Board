using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CommunityBoard.FrontEnd.Pages.Communication
{
    public class InboxModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public InboxModel(IConfiguration configuration)
        {
            _configuration = configuration;
            ChatUrl = _configuration["serviceUrl"] + _configuration["chatHub"];
        }

        public Message Message { get; set; }
        public string ChatUrl { get; }
        
        
        public void OnGet()
        {
            
        }
    }
}