using System.Collections.Generic;
using CommunityBoard.Core.Models.CoreModels;

namespace CommunityBoard.Core.Models.CommunicationModels
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
            Users = new HashSet<ChatUser>();
        }
        
        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> Users { get; set; }
    }
}