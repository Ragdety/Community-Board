﻿using CommunityBoard.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CommunityBoard.Core.Models.CoreModels;

namespace CommunityBoard.Core.Interfaces.Clients
{
	public interface IAnnouncementClient
    {
        Task<List<Announcement>> GetAnnouncementsAsync();
        Task<List<Announcement>> GetAnnouncementsByNameAsync(string name);
        Task<Announcement> GetAnnouncementByIdAsync(int id);
        Task<Tuple<List<Announcement>, string>> GetUserAnnouncementsAsync(int userId, string token);
        Task<bool> CreateAnnouncementAsync(CreateAnnouncementDto announcement, string token);
        Task<bool> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto announcement, string token);
        Task<bool> DeleteAnnouncementAsync(int id, string token);
    }
}