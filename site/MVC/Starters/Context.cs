using System.Globalization;
using System.IO;
using DFM.Generic;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Context
	{
		public static IHttpContextAccessor Accessor { get; private set; }

		public static void Configure(IServiceCollection services)
		{
			services.AddSession();
			services.AddHttpContextAccessor();
			var dir = new DirectoryInfo(@"data");

			services.AddDataProtection()
				.SetApplicationName("DfM")
				.PersistKeysToFileSystem(dir);
		}

		public static void Set(IApplicationBuilder app)
		{
			app.Use<Context>("Authorization", () => app.UseAuthorization());
			app.Use<Context>("Session", () => app.UseSession());

			Accessor = app.ApplicationServices
				.GetRequiredService<IHttpContextAccessor>();
		}

		public static void SetLanguage(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				Cfg.LanguagePath = "bin";

			app.Use<Context>("Language", async (context, next) =>
			{
				var current = context.GetService().Current;

				if (current.IsAuthenticated)
				{
					var cultureInfo = CultureInfo.GetCultureInfo(
						current.Language
					);

					CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
					CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

					context.StartTranslator();
				}

				await next();
			});
		}
	}
}
