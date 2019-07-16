using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Simple.Data;
using Simple.Hubs;

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

			services.AddDbContext<InMemoryDbContext>(options =>
				options.UseInMemoryDatabase(nameof(InMemoryDbContext))
			);

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddControllersWithViews().AddRazorRuntimeCompilation();
			services.AddRazorPages().AddRazorRuntimeCompilation();

			services.AddTransient<IEmployeeRepository, EmployeeRepository>();

			//services.AddAuthentication(options =>
			//{
			//	// Identity made Cookie authentication the default.
			//	// However, we want JWT Bearer Auth to be the default.
			//	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//})
			//.AddJwtBearer(options =>
			//{
			//	// Configure JWT Bearer Auth to expect our security key
			//	options.TokenValidationParameters =
			//				new TokenValidationParameters
			//				{
			//					LifetimeValidator = (before, expires, token, param) =>
			//					{
			//						return expires > DateTime.UtcNow;
			//					},
			//					ValidateAudience = false,
			//					ValidateIssuer = false,
			//					ValidateActor = false,
			//					ValidateLifetime = true,
			//					IssuerSigningKey = SecurityKey
			//				};

			//	// We have to hook the OnMessageReceived event in order to
			//	// allow the JWT authentication handler to read the access
			//	// token from the query string when a WebSocket or
			//	// Server-Sent Events request comes in.
			//	options.Events = new JwtBearerEvents
			//	{
			//		OnMessageReceived = context =>
			//		{
			//			var accessToken = context.Request.Query["access_token"];

			//			// If the request is for our hub...
			//			var path = context.HttpContext.Request.Path;
			//			if (!string.IsNullOrEmpty(accessToken) &&
			//				(path.StartsWithSegments("/hubs/chat")))
			//			{
			//				// Read the token out of the query string
			//				context.Token = accessToken;
			//			}
			//			return Task.CompletedTask;
			//		}
			//	};
			//});

			//services.AddCors(options => options.AddPolicy("CorsPolicy",
			//builder =>
			//{
			//	builder.AllowAnyMethod().AllowAnyHeader()
			//		   .WithOrigins("https://localhost:44337/")
			//		   .AllowCredentials();
			//}));

			//services.AddSignalR();

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

			services.AddSignalR();

			//services.AddSignalR()
			//	.AddMessagePackProtocol();

			//var hubConnection = new HubConnectionBuilder()
			//			.WithUrl("/chatHub")
			//			.AddMessagePackProtocol()
			//			.Build();

			//<script src="~/lib/signalr/signalr.js"></script>
			//<script src="~/lib/msgpack5/msgpack5.js"></script>
			//<script src="~/lib/signalr/signalr-protocol-msgpack.js"></script>

			//const connection = new signalR.HubConnectionBuilder()
			//	.withUrl("/chatHub")
			//	.withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
			//	.build();

			//services
			//	.AddSignalR(hubOptions =>
			//	{
			//		hubOptions.EnableDetailedErrors = true;
			//		hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
			//	}).AddMessagePackProtocol(options =>
			//	{
			//		options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
			//		{
			//			MessagePack.Resolvers.StandardResolver.Instance,
			//			//MessagePack.Resolvers.GeneratedResolver.Instance,
			//		};
			//	})
			//	.AddHubOptions<ChatHub>(options =>
			//	{
			//		options.EnableDetailedErrors = true;
			//	}).AddJsonProtocol(options =>
			//	{
			//		options.PayloadSerializerOptions.IgnoreNullValues = true;
			//	})
			//;

			// Change to use Name as the user identifier for SignalR
			// WARNING: This requires that the source of your JWT token
			// ensures that the Name claim is unique!
			// If the Name claim isn't unique, users could receive messages
			// intended for a different user!
			//services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

			// Change to use email as the user identifier for SignalR
			// services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();

			// WARNING: use *either* the NameUserIdProvider *or* the
			// EmailBasedUserIdProvider, but do not use both.

			//var connection = new HubConnectionBuilder()
			//	.WithUrl("https://example.com/myhub", options =>
			//	{
			//		options.UseDefaultCredentials = true;
			//	})
			//	.Build();

			// create a new user
			//var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
			//var result = await _userManager.CreateAsync(user, Input.Password);

			// add the email claim and value for this user
			//await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, Input.Email));

			//services.AddHostedService<Worker>();
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

			//app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseCookiePolicy();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			//Security considerations in ASP.NET Core SignalR

			//app.UseCors("CorsPolicy");

			// Make sure the CORS middleware is ahead of SignalR.
			//app.UseCors(builder =>
			//{
			//	builder.WithOrigins("https://example.com")
			//		.AllowAnyHeader()
			//		.WithMethods("GET", "POST")
			//		.AllowCredentials();
			//});

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

			//Logging
			//var connection = new HubConnectionBuilder()
			//	.WithUrl("https://example.com/my/hub/url")
			//	.ConfigureLogging(logging =>
			//	{
			//		// Log to the Console
			//		logging.AddConsole();
			//		logging.AddDebug();
			//		logging.AddProvider(new MyCustomLoggingProvider());

			//		// Set the default log level to Information, but to Debug for SignalR-related loggers.
			//		logging.SetMinimumLevel(LogLevel.Information);
			//		logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
			//		logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);

			//		// This will set ALL logging to Debug level
			//		logging.SetMinimumLevel(LogLevel.Debug);
			//	})
			//	.Build();

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

				endpoints.MapHub<SignalServer>("/signalServer");
				//endpoints.MapHub<ChatHub>("/chatHub");
				//endpoints.MapHub<ChatHub>("/hub");
				//endpoints.MapHub<ChatHub>("/hubs/clock");
			});
		}
	}
}
