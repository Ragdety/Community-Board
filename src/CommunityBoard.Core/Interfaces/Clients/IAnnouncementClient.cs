using CommunityBoard.Core.Models;
using CommunityBoard.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Clients
{
    public interface IAnnouncementClient
    {
        Task<List<Announcement>> GetAnnouncementsAsync();
        Task<Announcement> GetAnnouncementByIdAsync(int id);
        Task<Announcement> GetUserAnnouncementAsync(int userId);
        Task<bool> CreateAnnouncementAsync(CreateAnnouncementDto announcement);
        Task<bool> UpdateAnnouncementAsync(UpdateAnnouncementDto announcement);
        Task<bool> DeleteAnnouncementAsync(int id);
    }
}