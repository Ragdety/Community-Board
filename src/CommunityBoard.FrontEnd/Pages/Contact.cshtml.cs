using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBoard.Core.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IAnnouncementClient _apiAnnouncementClient;

        public ContactModel()
		{

		}

        public void OnGet(int announcementId)
        {

        }
    }
}