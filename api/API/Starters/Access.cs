using System;
using DFM.API.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;

namespace DFM.API.Starters
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
