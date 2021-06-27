using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace CommunityBoard.FrontEnd.Services.V1
{
    public class BaseClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseClient(
            HttpClient httpClient, 
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}