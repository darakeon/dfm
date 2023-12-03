using System.IO;
using DFM.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.API.Starters
{
	class Settings
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
