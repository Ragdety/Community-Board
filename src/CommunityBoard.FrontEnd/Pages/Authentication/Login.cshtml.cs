using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly IIdentityClient _identityClient;

        public LoginModel(IIdentityClient identityClient)
        {
            _identityClient = identityClient;
        }

        [BindProperty]
        public string EmailOrUsername { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _identityClient.Login(new UserLoginDto
            {
                EmailOrUserName = EmailOrUsername.Trim(),
                Password = Password.Trim()
            });

            if(!response.Success)
            {
                foreach (var error in response.Errors)
                {
                    //Add errors to Model State to display them using Validation Summary
                    ModelState.AddModelError("LoginErrors", error);
                }
                return Page();
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(response.Token);

            var userPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(token.Claims, "myClaims")
            );

            Response.Cookies.Append("JWToken", response.Token);

            //Sign the user in
            await HttpContext.SignInAsync(userPrincipal);
            return RedirectToPage("/Index");
        }
    }
}