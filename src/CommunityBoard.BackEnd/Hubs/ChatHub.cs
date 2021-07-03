using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Hubs
{
	public class ChatHub : Hub
	{
		public string GetConnectionId =>
			Context.ConnectionId;
	}
}