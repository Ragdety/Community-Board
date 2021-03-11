using System.Net.Http;

namespace CommunityBoard.FrontEnd.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddTokenToHeader(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            return client;
        }
    }
}