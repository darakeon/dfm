using System;
using DFM.API.Routes;
using DFM.BaseWeb.Routes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.API
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Route.AddUrl<Apis.Main>();
			Route.AddUrl<Apis.Object>();

			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(b => b.UseStartup<Startup>())
				.Build()
				.Run();
		}
	}
}
