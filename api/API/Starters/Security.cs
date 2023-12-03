using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFM.API.Starters
{
	class Security
	{
		public static void DenyFrame(IApplicationBuilder app)
		{
			app.Use<Security>(async (context, next) =>
			{
				context.Response.Headers
					.Append("X-Frame-Options", "deny");
				await next();
			});
		}
	}
}
