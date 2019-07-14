using Microsoft.AspNetCore.SignalR;

namespace Simple.Hubs
{
	public class NameUserIdProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			return connection.User?.Identity?.Name;
		}
	}
}
