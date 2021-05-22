using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Hubs
{
	public class ChatHub : Hub
	{
		// public async Task SendMessage(Message message) =>
		// 	await Clients.All.SendAsync("receiveMessage", message);
		//
	}
}