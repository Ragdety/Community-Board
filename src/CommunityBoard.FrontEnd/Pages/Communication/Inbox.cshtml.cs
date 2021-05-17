using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages.Communication
{
    public class InboxModel : PageModel
    {
        public InboxModel()
        {
            
        }

        public Message Message { get; set; }
        
        public void OnGet()
        {
            
        }
    }
}