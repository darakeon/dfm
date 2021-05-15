using DFM.Generic.Datetime;
using DFM.MVC.Models;
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
			BaseModel.IsDev = env.IsDevelopment();
			TZ.Init(env.IsDevelopment());

			Config.Initialize(env);

			Security.DenyFrame(app);

			StaticFiles.Configure(app);

			Route.CreateRoutes();

			Rewrite.TestThemAll(app);

			Context.Set(app);

			Error.AddHandlers(app, env);

			Orm.Config(app, life);

			Context.SetLanguage(app, env);

			Access.Run(app);

			Route.Execute(app);
		}
	}
}
