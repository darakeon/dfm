using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;

namespace DFM.MVC.Starters
{
	class Access
	{
		public static void Run(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				var service = context.GetService();
				var current = service.Current;

				if (current.IsAuthenticated)
					service.Access.Safe.SaveAccess();

				await next();
			});
		}
	}
}
