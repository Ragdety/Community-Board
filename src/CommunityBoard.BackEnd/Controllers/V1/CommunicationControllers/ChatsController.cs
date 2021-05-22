using System;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Extensions;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1.CommunicationControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatsController : Controller
    {
        private readonly IChatsRepository _chatsRepository;

        public ChatsController(IChatsRepository chatsRepository)
        {
            _chatsRepository = chatsRepository;
        }
        
        [HttpPost(ApiRoutes.Chats.CreateUserChat)]
        public async Task<IActionResult> CreateUserChat([FromRoute] int userId)
        {
            try
            {
                var rootUserId = HttpContext.GetUserId();
                await _chatsRepository.CreateUserChat(rootUserId, userId);
                
                var baseUrl = HttpContext.GetBaseURL();
                var locationUri = baseUrl + ApiRoutes.Chats.GetUserChat.Replace("{userId}", userId.ToString());
                //var chatResponse = new UserChatResponse() { UserId = userId};
                return Created(locationUri, new
                {
                    Created = true,
                    Message = "Chat successfully created.",
                    UserId = userId
                });
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
        }
        
        [HttpGet(ApiRoutes.Chats.GetUserChat)]
        public async Task<IActionResult> GetUserChat([FromRoute] int userId)
        {
            try
            {
                var rootUserId = HttpContext.GetUserId();
                var chat = await _chatsRepository.GetUserChat(rootUserId, userId);
                if (chat is null)
                    return NotFound(new {Message = "You do not have a chat with this user."});
                return Ok(chat);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized(new { ErrorMessage = "You must be logged in to access chat." });
            }
        }
        
        [HttpGet(ApiRoutes.Chats.GetAllUserChats)]
        public async Task<IActionResult> GetAllUserChats()
        {
            try
            {
                var rootUserId = HttpContext.GetUserId();
                return Ok(await _chatsRepository.GetAllUserChats(rootUserId));
            }
            catch (ArgumentNullException)
            {
                return Unauthorized(new { ErrorMessage = "You must be logged in to access your chats." });
            }
        }
    }
}