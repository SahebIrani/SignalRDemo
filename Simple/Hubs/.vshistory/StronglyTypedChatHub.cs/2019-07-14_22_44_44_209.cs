using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class StronglyTypedChatHub : Hub<IChatClient>
	{
		public async Task SendMessage(string user, string message)
		{
			await Clients.All.ReceiveMessage(user, message);
		}

		public Task SendMessageToCaller(string message)
		{
			return Clients.Caller.ReceiveMessage(message);
		}

		[HubMethodName("SendMessageToUser")]
		public Task DirectMessage(string user, string message)
		{
			return Clients.User(user).SendAsync("ReceiveMessage", message);
		}
	}
}
