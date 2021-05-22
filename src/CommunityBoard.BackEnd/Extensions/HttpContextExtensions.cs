using Microsoft.AspNetCore.Http;

namespace CommunityBoard.BackEnd.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetBaseURL(this HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}";
        }
    }
}