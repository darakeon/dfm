using System;
using DfM.Logs;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;

namespace DFM.MVC.Starters
{
	class Access
	{
		public static void Run(IApplicationBuilder app)
		{
			app.Use<Access>(async (context, next) =>
			{
				try
				{
					var service = context.GetService();
					var current = service.Current;

					if (current.IsAuthenticated)
						service.Access.Law.SaveAccess();
				}
				catch (Exception e)
				{
					e.TryLogHandled("Error on logging access");
				}

				await next();
			});
		}
	}
}
