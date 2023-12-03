using System;
using System.Threading.Tasks;
using DFM.API.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFM.API.Starters
{
	static class AppLog
	{
		public static void Use<T>(this IApplicationBuilder app, Action action)
		{
			app.Use<T>(null, action);
		}

		public static void Use<T>(this IApplicationBuilder app, String specific, Action action)
		{
			app.Use(async (context, next) =>
			{
				write<T>("start", specific, context);
				await next();
			});

			action();

			app.Use(async (context, next) =>
			{
				write<T>("end", specific, context);
				await next();
			});
		}

		public static void Use<T>(this IApplicationBuilder app, Func<HttpContext, Func<Task>, Task> middleware)
		{
			app.Use<T>(null, middleware);
		}

		public static void Use<T>(this IApplicationBuilder app, String specific, Func<HttpContext, Func<Task>, Task> middleware)
		{
			app.Use(async (context, next) =>
			{
				void writeState(String state)
				{
					write<T>(state, specific, context);
				}

				try
				{
					if (middleware == null)
					{
						writeState("call");
						await next();
					}
					else
					{
						writeState("start");
						await middleware(context, next);
						writeState("end");
					}
				}
				catch
				{
					writeState("fail");
					throw;
				}
			});
		}

		private static void write<T>(String state, String specific, HttpContext context)
		{
			if (!String.IsNullOrEmpty(specific))
				specific = $"[{specific}] ";

			context.AddLog(
				$"{state} {typeof(T).Name} {specific}" +
				$"at ${context.Request.Path}"
			);
		}

		public static void ShowLogOnError(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				context.InitLog();

				try
				{
					await next();
				}
				catch
				{
					context.GetLogs().ForEach(Console.WriteLine);
					throw;
				}
			});
		}
	}
}
