using System;
using System.Reflection;
using DFM.BaseWeb.Routes;
using DFM.Generic.Datetime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DFM.BaseWeb.Starters
{
	public class Startup
	{
		public static void Run<Program>(String[] args)
		{
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(
					builder => builder
						.UseStartup<Startup>()
						.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).GetTypeInfo().Assembly.FullName)
				)
				.Build()
				.Run();
		}

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
				.AddNewtonsoftJson(Serialization.Set);

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

			TZ.Init(env.IsDevelopment());

			Security.DenyFrame(app);

			StaticFiles.Configure(app);

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
