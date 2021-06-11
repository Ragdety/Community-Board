using System;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Extensions;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;
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
                //TODO: Add validation to make sure chat is not created twice
                var chat = new Chat();
                var rootUserId = HttpContext.GetUserId();
                await _chatsRepository.CreateUserChat(chat, rootUserId, userId);
                
                var baseUrl = HttpContext.GetBaseURL();
                var locationUri = baseUrl + ApiRoutes.Chats.GetChat.Replace("{chatId}", chat.Id.ToString());
                return Created(locationUri, new
                {
                    Created = true,
                    Message = "Chat successfully created.",
                    ChatId = chat.Id
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