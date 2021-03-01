using System.Collections.Generic;

namespace CommunityBoard.Core.DTOs.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}