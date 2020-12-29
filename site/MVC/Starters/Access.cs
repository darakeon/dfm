using System;
using DFM.Generic;
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
				try
				{
					var service = context.GetService();
					var current = service.Current;

					if (current.IsAuthenticated)
						service.Access.Safe.SaveAccess();
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
