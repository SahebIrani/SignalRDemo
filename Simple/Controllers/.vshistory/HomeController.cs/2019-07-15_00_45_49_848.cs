using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Simple.Models;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Simple.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHubContext<NotificationHub> _hubContext;

		public HomeController(IHubContext<NotificationHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task<IActionResult> Index()
		{
			await _hubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
