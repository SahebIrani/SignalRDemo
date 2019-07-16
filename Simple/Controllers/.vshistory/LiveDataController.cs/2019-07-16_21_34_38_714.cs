using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Simple.Data;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Controllers
{
	public class LiveDataController : Controller
	{
		public LiveDataController(InMemoryDbContext inMemoryDbContext) => InMemoryDbContext = inMemoryDbContext;

		public InMemoryDbContext InMemoryDbContext { get; }

		public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
		{
			IEnumerable<DataRealTime> result =
				await InMemoryDbContext.DataRealTime
						.AsNoTracking().ToListAsync(cancellationToken);

			return View(result);
		}

		[HttpGet]
		public async Task<IActionResult> CreateAsync(int id, CancellationToken cancellationToken)
		{
			if (id > 0)
			{
				DataRealTime result =
					await InMemoryDbContext.DataRealTime
						.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
			}
			else
			{

			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] DataRealTime data, CancellationToken cancellationToken)
		{
			if (data.Id > 0)
			{
				DataRealTime result =
					await InMemoryDbContext.DataRealTime
						.AsNoTracking().SingleOrDefaultAsync(c => c.Id == data.Id, cancellationToken);

				result.Title = data.Title;
				result.Price = data.Price;

				InMemoryDbContext.DataRealTime.Update(result);
				await InMemoryDbContext.SaveChangesAsync(cancellationToken);

				return View();
			}
			else
			{
				int resultId =
					await InMemoryDbContext.DataRealTime.AsNoTracking()
							.OrderByDescending(c => c.Id)
							.Select(c => c.Id)
							.FirstOrDefaultAsync(cancellationToken);

				DataRealTime dataRealTime = new DataRealTime
				{
					Id = resultId++,
					Title = data.Title,
					Price = data.Price,
				};

				await InMemoryDbContext.DataRealTime.AddAsync(dataRealTime, cancellationToken);
				await InMemoryDbContext.SaveChangesAsync(cancellationToken);

				return View();
			}
		}
	}
}
