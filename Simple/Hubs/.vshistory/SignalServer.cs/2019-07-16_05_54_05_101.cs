using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class SignalServer : Hub
	{
		public async Task NotifyMessage(string name, short age)
		{
			await Clients.All.SendAsync("ReciveNotify", name, age);
		}
	}
}
