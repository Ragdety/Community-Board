using System;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.GetUserById)]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            var user = await _userRepository.FindUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "User was not found." });
            return Ok(user);
        }
        
        [HttpGet(ApiRoutes.Users.Me)]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var user = await _userRepository.FindUserByIdAsync(userId);
                if (user == null)
                    return NotFound(new { Message = "User was not found." });
                return Ok(user);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("User is null");
            }
        }

        [HttpPut(ApiRoutes.Users.UpdateAvatar)]
        public async Task<IActionResult> UpdateAvatar(IFormFile image)
        {
            throw new NotImplementedException();
            // if (image == null) return BadRequest();
            //
            // var user = await _userRepository.FindUserByIdAsync(HttpContext.GetUserId());
            // if (user == null) return NotFound(new { Message = "User was not found" });
            //
            //
            //
            // return Ok(user);
        }
    }
}