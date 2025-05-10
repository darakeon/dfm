using System;
using DFM.BaseWeb.Starters;
using DFM.BaseWeb.Routes;
using DFM.MVC.Routes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Error.GetErrorUrl = () => Route.GetUrl<Default.Main>("Ops", "Code", "{0}");

			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(b => b.UseStartup<Startup>())
				.Build()
				.Run();
		}
	}
}
