using System;
using DFM.BaseWeb.Helpers.Global;
using DFM.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.BaseWeb.Starters
{
	public class Error
	{
		public static Func<String>? GetErrorUrl;

		public static void AddHandlers(IApplicationBuilder app, IWebHostEnvironment env)
		{
			var errorUrl = GetErrorUrl?.Invoke();

			if (errorUrl != null)
			{
				app.Use<Error>("StatusCode", () =>
					app.UseStatusCodePagesWithReExecute(errorUrl)
				);
			}

			if (env.IsDevelopment())
			{
				app.Use<Error>("DevException", () => app.UseDeveloperExceptionPage());
			}
			else
			{
				var handler = new ExceptionHandlerOptions
				{
					ExceptionHandler = exception => ErrorManager.Process(
						exception, LogFactory.Service,
						errorUrl?.Replace("{0}", "500")
					)
				};

				app.Use<Error>("Handler", () => app.UseExceptionHandler(handler));
				app.Use<Error>("HSTS", () => app.UseHsts());
			}
		}
	}
}
