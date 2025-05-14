﻿using System.IO;
using DFM.Generic;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Settings
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
