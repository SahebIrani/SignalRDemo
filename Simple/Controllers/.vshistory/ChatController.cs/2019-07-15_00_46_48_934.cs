using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Simple.Hubs;

using System.Threading.Tasks;

namespace Simple.Controllers
{
	public class ChatController : Controller
	{
		public IHubContext<ChatHub, IChatClient> _strongChatHubContext { get; }

		public ChatController(IHubContext<ChatHub, IChatClient> chatHubContext)
		{
			_strongChatHubContext = chatHubContext;
		}

		public async Task SendMessage(string message)
		{
			await _strongChatHubContext.Clients.All.ReceiveMessage(message);
		}
	}
}
