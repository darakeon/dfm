using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Security
	{
		public static void SetHttps(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (!env.IsDevelopment())
				app.UseHttpsRedirection();
		}

		public static void DenyFrame(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				context.Response.Headers
					.Add("X-Frame-Options", "deny");
				await next();
			});
		}
	}
}
