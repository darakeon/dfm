using DFM.API.Helpers.Global;
using DFM.Generic;
using DFM.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.API.Starters
{
	class Error
	{
		public static void AddHandlers(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.Use<Error>("DevException", () => app.UseDeveloperExceptionPage());
			}
			else
			{
				ILogService logService =
					Cfg.Log.Local
						? new LocalLogService()
						: new CloudWatchService();

				var handler = new ExceptionHandlerOptions
				{
					ExceptionHandler = exception => ErrorManager.Process(exception, logService)
				};

				app.Use<Error>("Handler", () => app.UseExceptionHandler(handler));
				app.Use<Error>("HSTS", () => app.UseHsts());
			}
		}
	}
}
