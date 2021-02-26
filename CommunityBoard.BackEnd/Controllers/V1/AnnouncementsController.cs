using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1
{
    public class AnnouncementsController : Controller
    {
        [HttpGet("api/v1/announcements")]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
