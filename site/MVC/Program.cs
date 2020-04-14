using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC
{
	public class Program
	{
		public static void Main(String[] args)
		{
			var host = CreateHostBuilder(args).Build();

			if (args.Contains("--start-and-end"))
				Startup.StopHostAfterConfig(host);

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(String[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(
					b => build(args, b)
				);

		private static void build(String[] args, IWebHostBuilder webBuilder)
		{
			webBuilder
				.UseUrls($"http://*:{getPort(args)}")
				.UseStartup<Startup>();
		}

		private static Int16 getPort(string[] args)
		{
			var regex = new Regex("p\\d+");

			var portArg = args.FirstOrDefault(
				a => regex.IsMatch(a)
			);

			if (portArg == null) return 80;

			var port = portArg.Substring(1);
			return Int16.Parse(port);
		}
	}
}
