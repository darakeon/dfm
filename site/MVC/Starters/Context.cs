using System.Globalization;
using DFM.Generic;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
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
		}

		public static void Set(IApplicationBuilder app)
		{
			app.UseAuthorization();
			app.UseSession();

			Accessor = app.ApplicationServices
				.GetRequiredService<IHttpContextAccessor>();
		}

		public static void SetLanguage(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				Cfg.LanguagePath = "bin";

			app.Use(async (context, next) =>
			{
				var current = context.GetService().Current;

				if (current.IsAuthenticated)
				{
					var cultureInfo = CultureInfo.GetCultureInfo(
						current.Language
					);

					CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
					CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

					context.GetTranslator();
				}

				await next();
			});
		}
	}
}
