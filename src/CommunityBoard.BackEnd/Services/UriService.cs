using System;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.Contracts.Requests.Queries;
using CommunityBoard.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace CommunityBoard.BackEnd.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        
        public Uri GetAnnouncementUri(string announcementId)
        {
            return new Uri(_baseUri + ApiRoutes.Announcements.Get.Replace("{announcementId}", announcementId));
        }

        public Uri GetAllAnnouncementsUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);
            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = string.Concat(_baseUri, ApiRoutes.Announcements.GetAll);   
            modifiedUri = 
                QueryHelpers.AddQueryString(modifiedUri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = 
                QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}