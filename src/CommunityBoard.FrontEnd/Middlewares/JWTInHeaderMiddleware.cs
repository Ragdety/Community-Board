using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Middlewares
{
    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var name = "JWToken";
            var cookieToken = context.Request.Cookies[name];

            if (cookieToken != null)
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers.Append("Authorization", "Bearer " + cookieToken);
            }

            await _next.Invoke(context);
        }
    }
}