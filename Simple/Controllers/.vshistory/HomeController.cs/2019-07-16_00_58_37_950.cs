using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
			IEmployeeRepository employeeRepository)
		{
			HubContext = hubContext;
			EmployeeRepository = employeeRepository;
		}

		public IHubContext<ChatHub> HubContext { get; }
		public IEmployeeRepository EmployeeRepository { get; }

		public async Task<IActionResult> Index()
		{
			await HubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
			return View();
		}

		[HttpGet]
		public IActionResult GetEmployees()
		{
			return Ok(EmployeeRepository.GetAllEmployees());
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
