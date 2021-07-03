using System;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Extensions;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBoard.BackEnd.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatsController : Controller
    {
        private readonly IChatsRepository _chatsRepository;

        public ChatsController(IChatsRepository chatsRepository)
        {
            _chatsRepository = chatsRepository;
        }

        [HttpGet(ApiRoutes.Chats.GetChat)]
        public async Task<IActionResult> Get(int chatId)
        {
            var chat = await _chatsRepository.FindByIdAsync(chatId);

            if (chat is null)
                return NotFound(new { Message = "Chat does not exist." });
            
            //Check if user in http context is in the chat
            if (!_chatsRepository.IsUserInChat(chat, HttpContext.GetUserId()))
                return Unauthorized(new { Message = "You do not own this chat" });
            
            return Ok(chat);
        }
        
        [HttpPost(ApiRoutes.Chats.CreateUserChat)]
        public async Task<IActionResult> CreateUserChat([FromRoute] int userId)
        {
            try
            {
                var rootUserId = HttpContext.GetUserId();
                var (item1, item2) = await _chatsRepository.UsersHaveChat(rootUserId, userId);
                
                //Correct? Might change this?
                //This works but...is it good practice?
                if (item1)
                    return Ok(new ChatResponse
                    {
                        Message = "Chat already exists.",
                        Code = 200,
                        Errors = new[] { "Chat could not be created" },
                        Id = item2.Id
                    });
                
                var chat = new Chat();
                await _chatsRepository.CreateUserChat(chat, rootUserId, userId);
                
                var baseUrl = HttpContext.GetBaseURL();
                var locationUri = baseUrl + ApiRoutes.Chats.GetChat.Replace("{chatId}", chat.Id.ToString());

                return Created(locationUri, new ChatResponse
                {
                    Message = "Chat successfully created.",
                    Code = 201,
                    Errors = null,
                    Id = chat.Id
                });
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
        }
        
        [HttpGet(ApiRoutes.Chats.GetAllUserChats)]
        public async Task<IActionResult> GetAllUserChats()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                return Ok(await _chatsRepository.FindAllUserChatsAsync(userId));
            }
            catch (ArgumentNullException)
            {
                return Unauthorized(new { ErrorMessage = "You must be logged in to access your chats." });
            }
        }
    }
}