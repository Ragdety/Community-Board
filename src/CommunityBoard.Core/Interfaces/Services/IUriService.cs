using System;
using CommunityBoard.Core.Contracts.Requests.Queries;

namespace CommunityBoard.Core.Interfaces.Services
{
    public interface IUriService
    {
        Uri GetAnnouncementUri(string announcementId);
        Uri GetAllAnnouncementsUri(PaginationQuery pagination = null);
    }
}