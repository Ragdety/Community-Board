﻿using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
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

        public async Task OnGet()
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
}