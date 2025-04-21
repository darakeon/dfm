using System;
using System.Threading.Tasks;
using DFM.Logs;
using DFM.Logs.Data.Application;
using DFM.MVC.Helpers.Extensions;
using Konkah.LibraryCSharpColorTerminal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DFM.MVC.Starters
{
	static class AppLog
	{
		public static void CommonLog(IServiceCollection services, IHostEnvironment env)
		{
			services.AddLogging((builder) =>
			{
				if (!env.IsDevelopment())
					builder.ClearProviders();

				builder.AddProvider(new ApplicationLoggerProvider());
			});
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

		public static void LogAllRequests(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				var text = $"{context.Request.Method} {context.Request.Path}";

				var color = getRequestColor(context, text);

				ConsoleColored.WriteLine($"{DateTime.Now:yyyy:MM:dd HH:mm:ss} START {text}", color);

				await next();

				ConsoleColored.WriteLine($"{DateTime.Now:yyyy:MM:dd HH:mm:ss} END   {text}", color);
			});
		}

		private static ConsoleColor getRequestColor(HttpContext context, String text)
		{
			return context.Request.Method switch
			{
				"GET" => ConsoleColor.Green,
				"POST" => ConsoleColor.Yellow,
				"PATCH" => ConsoleColor.Magenta,
				"PUT" => ConsoleColor.Blue,
				"DELETE" => ConsoleColor.Red,
				"HEAD" => ConsoleColor.Cyan,
				"OPTIONS" => ConsoleColor.White,
				_ => ConsoleColor.Gray,
			};
		}
	}
}
