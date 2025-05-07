using System.IO;
using DFM.Generic;
using Microsoft.Extensions.Hosting;

namespace DFM.BaseWeb.Starters
{
	public class Settings
	{
		public static void Initialize(IHostEnvironment env)
		{
			Directory.SetCurrentDirectory(env.ContentRootPath);

			var name = env.IsDevelopment()
				? null
				: env.EnvironmentName;

			Cfg.Init(name);
		}
	}
}
