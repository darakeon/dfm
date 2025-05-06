﻿using System;
using System.IO;
using DFM.BaseWeb.Starters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace DFM.MVC.Starters
{
	class StaticFiles
	{
		public static void Configure(IApplicationBuilder app)
		{
			addStaticPath(app, "Assets");
			addStaticPath(app, ".well-known");
		}

		private static void addStaticPath(IApplicationBuilder app, String folder)
		{
			if (!Directory.Exists(folder))
				return;

			app.Use<StaticFiles>("StaticFiles", () =>
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
