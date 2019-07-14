using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessage(string user, string message)
			=> await Clients.All.SendAsync("ReceiveMessage", user, message);

		public async Task NewMessage(long username, string message)
			=> await Clients.All.SendAsync("messageReceived", username, message);
	}
}