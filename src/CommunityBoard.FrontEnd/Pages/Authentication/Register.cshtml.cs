using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages.Authentication
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly IIdentityClient _identityClient;

        public RegisterModel(IIdentityClient identityClient)
        {
            _identityClient = identityClient;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


        public async Task<IActionResult> OnPost()
        {
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(
                    "noMatchingPasswords", "Passwords must match");
                return Page();
            }

            var response = await _identityClient.Register(new UserRegistrationDto
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                UserName = UserName.Trim(),
                Email = Email.Trim(),
                Password = Password.Trim(),
                DateRegistered = DateTime.UtcNow
            });

            if (!response.Success)
            {
                foreach (var error in response.Errors)
                {
                    //Add errors to Model State to display them using Validation Summary
                    ModelState.AddModelError("RegistrationErrors", error);
                }
                return Page();
            }

            //Might send confirmation email

            

            return RedirectToPage("/Authentication/Login");
        }
    }
}