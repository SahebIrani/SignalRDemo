using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class ChatHub : Hub
	{
		public async Task NewMessage(long username, string message)
			=> await Clients.All.SendAsync("messageReceived", username, message);

		public Task SendMessage(string user, string message)
		{
			return Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public Task SendMessageToCaller(string message)
		{
			return Clients.Caller.SendAsync("ReceiveMessage", message);
		}

		public Task SendMessageToGroup(string message)
		{
			return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
		}

		public async Task AddToGroup(string groupName)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

			await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
		}

		public async Task RemoveFromGroup(string groupName)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

			await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
		}

		public Task SendPrivateMessage(string user, string message)
		{
			return Clients.User(user).SendAsync("ReceiveMessage", message);
		}
	}
}