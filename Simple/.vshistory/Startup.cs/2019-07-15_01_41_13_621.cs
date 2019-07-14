using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Simple.Data;
using Simple.Hubs;

using System;

namespace Simple
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddControllersWithViews();
			services.AddRazorPages();


			services.AddCors(options => options.AddPolicy("CorsPolicy",
			builder =>
			{
				builder.AllowAnyMethod().AllowAnyHeader()
					   .WithOrigins("https://localhost:44337/")
					   .AllowCredentials();
			}));

			services.AddSignalR();

			//services.AddSignalR()
			//	.AddJsonProtocol(options =>
			//	{
			//		options.PayloadSerializerSetting.ContractResolver =
			//		new DefaultContractResolver();
			//	});

			//var connection = new HubConnectionBuilder()
			//	.AddJsonProtocol(options =>
			//	{
			//		options.PayloadSerializerSettings.ContractResolver =
			//			new DefaultContractResolver();
			//	})
			//	.Build();

			//services.AddSignalR().AddHubOptions<ChatHub>(options =>
			//{
			//	options.EnableDetailedErrors = true;
			//});

			services
				.AddSignalR(hubOptions =>
				{
					hubOptions.EnableDetailedErrors = true;
					hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
				}).AddHubOptions<ChatHub>(options =>
				{
					options.EnableDetailedErrors = true;
				}).AddJsonProtocol(options =>
				{
					options.PayloadSerializerOptions.IgnoreNullValues = true;
				})
			;

			services.AddHostedService<Worker>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseCookiePolicy();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCors("CorsPolicy");

			//app.UseSignalR(options =>
			//{
			//	options.MapHub<ChatHub>("/hub");
			//});

			//app.UseSignalR((configure) =>
			//{
			//	var desiredTransports =
			//		HttpTransportType.WebSockets |
			//		HttpTransportType.LongPolling;

			//	configure.MapHub<ChatHub>("/myhub", (options) =>
			//	{
			//		options.Transports = desiredTransports;
			//	});
			//});

			//Configure logging
			//var connection = new HubConnectionBuilder()
			//.WithUrl("https://example.com/myhub")
			//.ConfigureLogging(logging => {
			//	logging.SetMinimumLevel(LogLevel.Information);
			//	logging.AddConsole();
			//})
			//.Build();

			//HubConnection hubConnection = HubConnectionBuilder.create("https://example.com/myhub")
			//.withTransport(TransportEnum.WEBSOCKETS)
			//.build();

			//Configure bearer authentication
			//var connection = new HubConnectionBuilder()
			//.WithUrl("https://example.com/myhub", options => {
			//	options.AccessTokenProvider = async () => {
			//		// Get and return the access token.
			//	};
			//})
			//.Build();

			//HubConnection hubConnection = HubConnectionBuilder.create("https://example.com/myhub")
			//.withAccessTokenProvider(Single.defer(()-> {
			//	// Your logic here.
			//	return Single.just("An Access Token");
			//})).build();

			//Configure additional options
			//var connection = new HubConnectionBuilder()
			//.WithUrl("https://example.com/myhub", options => {
			//	options.Headers["Foo"] = "Bar";
			//	options.Cookies.Add(new Cookie(/* ... */);
			//	options.ClientCertificates.Add(/* ... */);
			//})
			//.Build();

			//Authentication and authorization in ASP.NET Core SignalR

			//this.connection = new signalR.HubConnectionBuilder()
			//.withUrl("/hubs/chat", { accessTokenFactory: () => this.loginToken })
			//.build();

			//var connection = new HubConnectionBuilder()
			//.WithUrl("https://example.com/myhub", options =>
			//{
			//	options.AccessTokenProvider = () => Task.FromResult(_myAccessToken);
			//})
			//.Build();




			//app.Use(async (context, next) =>
			//{
			//	var hubContext = context.RequestServices
			//						.GetRequiredService<IHubContext<ChatHub>>();
			//});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();

				endpoints.MapHub<ChatHub>("/chatHub");
				endpoints.MapHub<ChatHub>("/hub");
				endpoints.MapHub<ChatHub>("/hubs/clock");
			});
		}
	}
}
