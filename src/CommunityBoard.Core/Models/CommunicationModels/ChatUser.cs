using CommunityBoard.Core.Models.CoreModels;

namespace CommunityBoard.Core.Models.CommunicationModels
{
    public class ChatUser
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}