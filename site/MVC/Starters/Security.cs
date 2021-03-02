using Microsoft.AspNetCore.Builder;

namespace DFM.MVC.Starters
{
	class Security
	{
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
