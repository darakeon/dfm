using System.IO;
using DFM.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Config
	{
		public static void Initialize(IWebHostEnvironment env)
		{
			Directory.SetCurrentDirectory(env.ContentRootPath);

			var name = env.IsDevelopment()
				? null
				: env.EnvironmentName;

			Cfg.Init(name);
		}
	}
}
