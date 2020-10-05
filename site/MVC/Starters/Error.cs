using DFM.MVC.Helpers.Global;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Error
	{
		public static void AddHandlers(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseStatusCodePagesWithReExecute(
				Route.GetUrl<Default.Main>("Ops", "Code", "{0}")
			);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				var handler = new ExceptionHandlerOptions
				{
					ExceptionHandler = ErrorManager.Process
				};

				app.UseExceptionHandler(handler);
				app.UseHsts();
			}
		}
	}
}
