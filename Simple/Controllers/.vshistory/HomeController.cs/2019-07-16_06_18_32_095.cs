using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Simple.Data;
using Simple.Hubs;
using Simple.Models;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Simple.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(
			IHubContext<ChatHub> hubContext,
			IHubContext<SignalServer> hubContextSignalServer,
			IEmployeeRepository employeeRepository,
			ApplicationDbContext context
		)
		{
			HubContext = hubContext;
			HubContextSignalServer = hubContextSignalServer;
			EmployeeRepository = employeeRepository;
			Context = context;
		}

		public IHubContext<ChatHub> HubContext { get; }
		public IHubContext<SignalServer> HubContextSignalServer { get; }
		public IEmployeeRepository EmployeeRepository { get; }
		public ApplicationDbContext Context { get; }

		public async Task<IActionResult> Index()
		{
			//await HubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
			//await HubContextSignalServer.Clients.All.SendAsync("ReciveNotifyFromController", $"Home page loaded at: {DateTime.Now}");
			return View();
		}

		public IActionResult GetEmployees()
		{
			return Ok(EmployeeRepository.GetAllEmployees());
		}

		public async Task<IActionResult> ReciveMethod()
		{
			await HubContextSignalServer.Clients.All.SendAsync("ReciveNotifyFromController", $"Home page loaded at: {DateTime.Now}");

			return View(nameof(Index));
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
