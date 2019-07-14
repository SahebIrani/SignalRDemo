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
	}
}