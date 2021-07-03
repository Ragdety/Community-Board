using System;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.BackEnd.Hubs;
using CommunityBoard.Core.DTOs.CommunicationDTOs;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CommunityBoard.BackEnd.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController : Controller
    {
        private readonly IMessagesRepository _messagesRepository;
        private readonly IHubContext<ChatHub> _chat;

        public MessagesController(
            IMessagesRepository messagesRepository, 
            IHubContext<ChatHub> chat)
        {
            _messagesRepository = messagesRepository;
            _chat = chat;
        }
        
        [HttpPost(ApiRoutes.Messages.Send)]
        public async Task<IActionResult> SendMessage(
            [FromRoute] int chatId, 
            [FromBody] CreateMessageDto createMessageDto)
        {
            if (string.IsNullOrEmpty(createMessageDto.Text))
                return BadRequest(new { Message = "Message cannot be empty." });
            
            var message = new Message
            {
                Text = createMessageDto.Text,
                Timestamp = DateTime.UtcNow,
                UserName = User.FindFirst("username")?.Value,
                ChatId = chatId
            };

            await _messagesRepository.CreateAsync(message);
            
            //SignalR stuff for realtime functionality here:
            await _chat.Clients
                .Group(chatId.ToString())
                .SendAsync("ReceiveMessage", new
            {
                message.Text,
                message.Timestamp,
                message.UserName
            });

            return Ok(new { Message = "Message was sent to chat successfully." });
        }
    }
}