using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.API
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(b => b.UseStartup<Startup>())
				.Build()
				.Run();
		}
	}
}
