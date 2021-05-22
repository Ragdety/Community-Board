using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Controllers.V1.CommunicationControllers
{
    public class IdentityController : Controller
    {
        public readonly IIdentityRepository _identityRepository;

        public IdentityController(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            var authResponse = await _identityRepository.RegisterAsync(
                    user.FirstName, user.LastName, 
                    user.UserName, user.Email, user.Password);

            if(!authResponse.Success)
            {
                //Return fail response with error msgs to handle in front end
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            //Return the token to handle in front end
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            var authResponse = await _identityRepository.LoginAsync(
                user.EmailOrUserName, user.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpGet(ApiRoutes.Identity.GetUser)]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
		{
            var user = await _identityRepository.FindUserById(userId);

            if (user == null)
                return NotFound(new { Message = "User was not found." });

            return Ok(user);
		}

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var authResponse = 
                await _identityRepository.RefreshTokenAsync(
                    refreshTokenDto.Token, 
                    refreshTokenDto.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}
