using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Simple.Hubs;

using System.Threading.Tasks;

namespace Simple.Controllers
{
	public class ChatController : Controller
	{
		public IHubContext<StronglyTypedChatHub, IChatClient> StrongChatHubContext { get; }

		public ChatController(IHubContext<StronglyTypedChatHub, IChatClient> strongChatHubContext)
			=> StrongChatHubContext = strongChatHubContext;

		public async Task SendMessage(string message)
		{
			await _strongChatHubContext.Clients.All.ReceiveMessage(message);
		}
	}
}
