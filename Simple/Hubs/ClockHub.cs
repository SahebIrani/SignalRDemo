using Microsoft.AspNetCore.SignalR;

using System;
using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class ClockHub : Hub<IClock>
	{
		public async Task SendTimeToClients(DateTime dateTime)
		{
			await Clients.All.ShowTime(dateTime);
		}
	}
}
