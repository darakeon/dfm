using DFM.API.Models;
using DFM.API.Starters;
using DFM.API.Starters.Routes;
using DFM.Generic.Datetime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DFM.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			PrometheusConfig.Start();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime.
		// Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			Route.Configure(services);
			Context.Configure(services);

			services.AddControllers()
				.AddNewtonsoftJson(Serialization.Set);

			services.AddAntiforgery();
		}

		// This method gets called by the runtime.
		// Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
		{
			PrometheusConfig.UseMetrics(app);

			AppLog.ShowLogOnError(app);

			BaseModel.IsDev = env.IsDevelopment();
			TZ.Init(env.IsDevelopment());

			Settings.Initialize(env);

			Security.DenyFrame(app);

			StaticFiles.Configure(app);

			Route.CreateRoutes();

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
