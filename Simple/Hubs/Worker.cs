using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
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
		private readonly ILogger<ClockHubClient> _logger;
		private HubConnection _connection;

		public ClockHubClient(ILogger<ClockHubClient> logger)
		{
			_logger = logger;

			//_connection = new HubConnectionBuilder()
			//	.WithUrl(Strings.HubUrl)
			//	.Build();

			//_connection.On<DateTime>(Strings.Events.TimeSent,
			//	dateTime => _ = ShowTime(dateTime));
		}

		public Task ShowTime(DateTime currentTime)
		{
			_logger.LogInformation($"{currentTime.ToShortTimeString()}");

			return Task.CompletedTask;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			// Loop is here to wait until the server is running
			while (true)
			{
				try
				{
					await _connection.StartAsync(cancellationToken);

					break;
				}
				catch
				{
					await Task.Delay(1000);
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return _connection.DisposeAsync();
		}
	}
}
