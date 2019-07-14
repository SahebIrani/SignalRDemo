using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Simple
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureLogging(logging =>
					{
						logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
						logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
					})
					.UseStartup<Startup>();
				});
	}
}
