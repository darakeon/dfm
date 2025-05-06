using System;
using DFM.BaseWeb.Helpers.Extensions;
using DFM.BaseWeb.Starters;
using Microsoft.AspNetCore.Builder;

namespace DFM.MVC.Starters
{
	class Access
	{
		public static void Run(IApplicationBuilder app)
		{
			app.Use<Access>(async (context, next) =>
			{
				var service = context.GetService();

				try
				{
					var current = service.Current;

					if (current.IsAuthenticated)
						service.Access.Law.SaveAccess();
				}
				catch (Exception e)
				{
					service.LogService.LogHandled(e, "Error on logging access");
				}

				await next();
			});
		}
	}
}
