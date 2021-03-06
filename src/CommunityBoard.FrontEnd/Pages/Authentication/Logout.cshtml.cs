using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityBoard.FrontEnd.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            DeleteCookies();
            return RedirectToPage("/Index");
        }

        private void DeleteCookies()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }
    }
}