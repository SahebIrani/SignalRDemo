using Microsoft.AspNetCore.Mvc;

namespace Simple.Controllers
{
	public class LiveDataController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
