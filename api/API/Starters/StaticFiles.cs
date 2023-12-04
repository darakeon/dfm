using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace DFM.API.Starters
{
	class StaticFiles
	{
		public static void Configure(IApplicationBuilder app)
		{
			addStaticPath(app, "Assets");
		}

		private static void addStaticPath(IApplicationBuilder app, String folder)
		{
			if (!Directory.Exists(folder))
				return;

			app.Use<StaticFiles>(() =>
			{
				app.UseStaticFiles(new StaticFileOptions
				{
					FileProvider = new PhysicalFileProvider(
						Path.Combine(Directory.GetCurrentDirectory(), folder)
					),
					RequestPath = "/" + folder,
				});
			});
		}
	}
}
