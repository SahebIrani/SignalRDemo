using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Simple.Data;

using System.Collections.Generic;
using System.Threading;

namespace Simple.Controllers
{
	public class LiveDataController : Controller
	{
		public LiveDataController(InMemoryDbContext inMemoryDbContext) => InMemoryDbContext = inMemoryDbContext;

		public InMemoryDbContext InMemoryDbContext { get; }

		public async System.Threading.Tasks.Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
		{
			IEnumerable<DataRealTime> result =
				await InMemoryDbContext.DataRealTime
				.AsNoTracking().ToListAsync(cancellationToken);

			return View(result);
		}
	}
}
