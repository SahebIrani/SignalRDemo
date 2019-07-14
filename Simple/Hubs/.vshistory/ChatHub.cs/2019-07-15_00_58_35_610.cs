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

		public int GetTotalLength(string param1)
		{
			return param1.Length;
		}
		public int GetTotalLength(string param1, string param2)
		{
			return param1.Length + param2.Length;
		}
		public class TotalLengthRequest
		{
			public string Param1 { get; set; }
		}
		public int GetTotalLength(TotalLengthRequest req)
		{
			return req.Param1.Length;
		}

		public class TotalLengthRequest2
		{
			public string Param1 { get; set; }
			public string Param2 { get; set; }
		}

		public async Task<int> GetTotalLength(TotalLengthRequest2 req)
		{
			var length = req.Param1.Length;
			if (req.Param2 != null)
			{
				length += req.Param2.Length;
			}
			return length;
		}

		public async Task Broadcast(string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", new
			{
				Message = message
			});
		}
	}
}