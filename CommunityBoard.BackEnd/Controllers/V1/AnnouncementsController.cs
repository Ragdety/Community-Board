using CommunityBoard.BackEnd.Contracts.V1;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1
{
    public class AnnouncementsController : Controller
    {
        [HttpGet(ApiRoutes.Announcements.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(new { name = "test" });
        }
    }
}
