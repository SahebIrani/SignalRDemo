using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Simple.Data;
using Simple.Hubs;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Controllers
{
	public class LiveDataController : Controller
	{
		public LiveDataController(InMemoryDbContext inMemoryDbContext, IHubContext<LiveDataHub> hubContext)
		{
			InMemoryDbContext = inMemoryDbContext;
			HubContext = hubContext;
		}

		public InMemoryDbContext InMemoryDbContext { get; }
		public IHubContext<LiveDataHub> HubContext { get; }

		public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
		{
			IEnumerable<DataRealTime> result =
				await InMemoryDbContext.DataRealTime
						.AsNoTracking().ToListAsync(cancellationToken);

			return View(result);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
		{
			if (id > 0)
			{
				DataRealTime result =
					await InMemoryDbContext.DataRealTime
						.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

				return View(result);
			}
			else
			{
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(DataRealTime data, CancellationToken cancellationToken)
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
				//int resultId =
				//	await InMemoryDbContext.DataRealTime.AsNoTracking()
				//			.OrderByDescending(c => c.Id)
				//			.Select(c => c.Id)
				//			.FirstOrDefaultAsync(cancellationToken);

				DataRealTime dataRealTime = new DataRealTime
				{
					Id = new Random().Next(1, 9999),
					//Id = resultId++,
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
