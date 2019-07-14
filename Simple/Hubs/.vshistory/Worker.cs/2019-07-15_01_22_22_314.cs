using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Hubs
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IHubContext<ClockHub, IClock> _clockHub;

		public Worker(ILogger<Worker> logger, IHubContext<ClockHub, IClock> clockHub)
		{
			_logger = logger;
			_clockHub = clockHub;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation($"Worker running at: {DateTime.Now}");
				await _clockHub.Clients.All.ShowTime(DateTime.Now);
				await Task.Delay(1000);
			}
		}
	}

	public partial class ClockHubClient : IClock, IHostedService
	{
		public Task ShowTime(DateTime currentTime)
		{
			throw new NotImplementedException();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}


}
