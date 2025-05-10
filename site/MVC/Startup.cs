using DFM.BaseWeb.Starters;
using DFM.BaseWeb.Routes;
using DFM.Generic.Datetime;
using DFM.MVC.Models;
using DFM.MVC.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;

			PrometheusConfig.Start();
		}

		public IConfiguration Configuration { get; }
		public IHostEnvironment Environment { get; }

		// This method gets called by the runtime.
		// Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			Settings.Initialize(Environment);

			Route.Configure(services);
			Context.Configure(services);

			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddAntiforgery();

			AppLog.CommonLog(services, Environment);
		}

		// This method gets called by the runtime.
		// Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
		{
			AppLog.LogAllRequests(app, env);

			PrometheusConfig.UseMetrics(app);

			AppLog.ShowLogOnError(app);

			BaseModel.IsDev = env.IsDevelopment();
			TZ.Init(env.IsDevelopment());

			Security.DenyFrame(app);

			StaticFiles.Configure(app);

			Route.AddUrl<Default.Robots>();
			Route.AddUrl<Default.SiteMap>();
			Route.AddUrl<Accounts>();
			Route.AddUrl<Default.Contract>();
			Route.AddUrl<Default.Misc>();
			Route.AddUrl<Default.Mail>();
			Route.AddUrl<Default.Mobile>();
			Route.AddUrl<Default.Main>();

			Rewrite.TestThemAll(app);

			Context.Set(app);

			Error.AddHandlers(app, env);

			Orm.Configure(app, life);

			Context.SetLanguage(app, env);

			Access.Run(app);

			Route.Execute(app);
		}
	}
}
