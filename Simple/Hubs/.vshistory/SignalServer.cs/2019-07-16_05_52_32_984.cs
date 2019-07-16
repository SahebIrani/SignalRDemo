using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class SignalServer : Hub
	{
		public async Task NewMessage(string name, short name)
		{
			await Clients.All.SendAsync("RefreshEmployees", name, name);
		}
	}
}
