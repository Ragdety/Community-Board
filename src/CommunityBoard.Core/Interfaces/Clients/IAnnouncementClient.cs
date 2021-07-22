using CommunityBoard.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Models.CoreModels;

namespace CommunityBoard.Core.Interfaces.Clients
{
	public interface IAnnouncementClient
    {
        Task<List<Announcement>> GetAnnouncementsAsync();
        Task<List<Announcement>> GetAnnouncementsByNameAsync(string name);
        Task<Announcement> GetAnnouncementByIdAsync(int id);
        Task<Tuple<List<Announcement>, string>> GetUserAnnouncementsAsync();
        Task<bool> CreateAnnouncementAsync(CreateAnnouncementDto announcement);
        Task<bool> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto announcement);
        Task<bool> DeleteAnnouncementAsync(int id);
    }
}