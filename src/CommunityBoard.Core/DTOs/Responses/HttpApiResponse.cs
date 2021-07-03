using System.Collections.Generic;

namespace CommunityBoard.Core.DTOs.Responses
{
    public class HttpApiResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}