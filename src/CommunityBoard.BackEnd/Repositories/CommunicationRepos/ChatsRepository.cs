﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Repositories.CommunicationRepos
{
    public class ChatsRepository : GenericRepository<Chat>, IChatsRepository
    {
        private readonly DbSet<Chat> _chats;
        private readonly DbSet<ChatUser> _chatUsers;
        
        public ChatsRepository(ApplicationDbContext db) : base(db)
        {
            _chats = db.Set<Chat>();
            _chatUsers = db.Set<ChatUser>();
        }

        public override async Task<Chat> FindByIdAsync(object id)
        {
            var chat = await base.FindByIdAsync(id);
            await _db.Entry(chat).Collection(c => c.Users).LoadAsync();
            await _db.Entry(chat).Collection(c => c.Messages).LoadAsync();
            return chat;
        }

        public async Task<bool> JoinChat(int chatId, int userId)
        {
            var chatToAddUser = await FindByIdAsync(chatId);
            if (chatToAddUser is null)
                return false;
            
            chatToAddUser.Users.Add(new ChatUser
            {
                UserId = userId,
                ChatId = chatToAddUser.Id
            });

            var saved = await _db.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> CreateUserChat(Chat chat, int rootUserId, int targetUserId)
        {
            if (!await CreateAsync(chat))
                return false;

            if (!await JoinChat(chat.Id, rootUserId))
                return false;
            
            return await JoinChat(chat.Id, targetUserId);
        }

        public async Task<IEnumerable<Chat>> GetAllUserChats(int userId)
        {
            var user = 
                await _chatUsers.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user is null)
                return null;
            return await _chats
                .Include(x => x.Users)
                //.Include(y => y.Messages)
                .Where(x => x.Users.Contains(user))
                .ToListAsync();
        }

        public async Task<bool> DeleteUserChat(int chatId, int userId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUserInChat(Chat chat, int userId)
        {
            return chat.Users.FirstOrDefault(x => x.UserId == userId) != null;
        }
    }
}