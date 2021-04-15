using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Starters
{
	static class AppLog
	{
		public static void Use<T>(this IApplicationBuilder app, Func<HttpContext, Func<Task>, Task> middleware)
		{
			app.Use(async (context, next) =>
			{
				try
				{
					write<T>(context, "start");
					await middleware(context, next);
					write<T>(context, "end");
				}
				catch
				{
					write<T>(context, "fail");
					throw;
				}
			});
		}

		private static void write<T>(HttpContext context, String state)
		{
			Console.WriteLine(
				$"{state} {typeof(T).Name} " +
				$"at ${context.Request.Path}"
			);
		}
	}
}
