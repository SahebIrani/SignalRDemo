using Microsoft.AspNetCore.SignalR;

using System;
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
		public async Task DirectMessage(string user, string message)
		{
			await Clients.User(user).ReceiveMessage("ReceiveMessage", message);
		}

		public override async Task OnConnectedAsync()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
			await base.OnConnectedAsync();
		}
		public override async Task OnDisconnectedAsync(Exception exception)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
			await base.OnDisconnectedAsync(exception);
		}

		public Task ThrowException()
		{
			throw new HubException("This error will be sent to the client!");
		}
	}
}
