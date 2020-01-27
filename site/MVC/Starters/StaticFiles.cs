using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace DFM.MVC.Starters
{
	class StaticFiles
	{
		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			addStaticPath(app, "Assets");
		}

		public static void Certificate(IApplicationBuilder app)
		{
			addStaticPath(app, ".well-known");
		}

		private static void addStaticPath(IApplicationBuilder app, String folder)
		{
			if (!Directory.Exists(folder))
				return;

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(Directory.GetCurrentDirectory(), folder)),
				RequestPath = "/" + folder,
			});
		}
	}
}
