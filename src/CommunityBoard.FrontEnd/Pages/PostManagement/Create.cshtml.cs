using System.IO;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.FrontEnd.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages.PostManagement
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IAnnouncementClient _announcementClient;

        public CreateModel(IAnnouncementClient announcementClient)
        {
            _announcementClient = announcementClient;
        }

        public string Name { get; set; }

        public AnnouncementType Type { get; set; }

        public string Description { get; set; }

        [AllowedImageExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var createdAnnouncement = new CreateAnnouncementDto
            {
                Name = Name,
                Type = Type.ToString(),
                Description = Description,
                Image = null
            };

            if (ImageFile != null)
            {
                //Checking AllowedImageExtensions attribute error message
                if (!ModelState.IsValid)
                    return Page();

                MemoryStream ms = new MemoryStream();
                await ImageFile.CopyToAsync(ms);

                createdAnnouncement.Image = ms.ToArray();

                ms.Close();
                ms.Dispose();
            }

            var token = Request.Cookies["JWToken"];

            var success = await _announcementClient.CreateAnnouncementAsync(createdAnnouncement, token);
            if(!success)
            {
                ModelState.AddModelError("UnknownError", "Something went wrong creating the announcement...");
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}