using DFM.MVC.Starters;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC
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
			Route.Configure(services);
			Context.Configure(services);

			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddAntiforgery();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		// ReSharper disable once UnusedMember.Global
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
		{
			Config.Initialize(env);

			Security.DenyFrame(app);

			Security.SetHttps(app, env);

			Route.CreateRoutes();

			Rewrite.TestThemAll(app);

			Error.AddHandlers(app, env);

			Context.Set(app);

			StaticFiles.Configure(app, env);

			StaticFiles.Certificate(app);

			Orm.Set(app, env, life);

			Context.SetLanguage(app, env);

			Robot.Run(app, env, life);

			Route.Execute(app);

			Mobile.ConfigureIP();
		}
	}
}
