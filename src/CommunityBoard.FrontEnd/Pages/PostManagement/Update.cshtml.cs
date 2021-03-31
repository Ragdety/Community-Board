using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using CommunityBoard.FrontEnd.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages.PostManagement
{
    public class UpdateModel : PageModel
    {
        private readonly IAnnouncementClient _apiAnnouncementClient;

        public Announcement OldAnnouncement { get; set; }

        [BindProperty]
        public int CurrentAnnouncementId { get; set; }

		[BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public AnnouncementType Type { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        [AllowedImageExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile ImageFile { get; set; }

        public UpdateModel(IAnnouncementClient apiAnnouncementClient)
		{
			_apiAnnouncementClient = apiAnnouncementClient;
		}

		public async Task<IActionResult> OnGetAsync(int id)
        {
            CurrentAnnouncementId = id;
            OldAnnouncement = await _apiAnnouncementClient.GetAnnouncementByIdAsync(CurrentAnnouncementId);
            if (OldAnnouncement == null)
                return RedirectToPage("/Errors/NotFound");

            MapAnnouncementProperties();
            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            OldAnnouncement = await _apiAnnouncementClient.GetAnnouncementByIdAsync(CurrentAnnouncementId);
            var token = Request.Cookies["JWToken"];

            var updateAnnouncementDto = new UpdateAnnouncementDto
            {
                Name = Name,
                Type = Type.ToString(),
                Description = Description,
                Image = OldAnnouncement.Image
            };


            if (ImageFile != null)
            {
                if (!ModelState.IsValid)
                    return Page();

                MemoryStream ms = new MemoryStream();
                await ImageFile.CopyToAsync(ms);

                updateAnnouncementDto.Image = ms.ToArray();

                ms.Close();
                ms.Dispose();
            }

            var success = await _apiAnnouncementClient.UpdateAnnouncementAsync(
                OldAnnouncement.Id, updateAnnouncementDto, token);

            if (!success)
            {
                ModelState.AddModelError("UnknownError", "Something went wrong updating the announcement...");
                return Page();
            }
            return RedirectToPage("/PostManagement/Manage");
        }

        private void MapAnnouncementProperties()
		{
            Name = OldAnnouncement.Name;
            Type = OldAnnouncement.Type;
            Description = OldAnnouncement.Description;
        }
    }
}