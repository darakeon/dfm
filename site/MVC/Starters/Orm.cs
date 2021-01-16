using System;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using DfM.Logs;
using Keon.MVC.Cookies;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Orm
	{
		public static void Config(IApplicationBuilder app, IHostApplicationLifetime life)
		{
			try
			{
				SessionFactoryManager.Initialize<UserMap, User>(Cfg.DB);

				app.Use(async (context, next) =>
				{
					SessionManager.Init(
						() => BrowserId.Get(() => context)
					);

					await next();

					SessionManager.Close();
				});

				life.ApplicationStopping.Register(SessionFactoryManager.End);
			}
			catch (Exception e)
			{
				e.TryLogHandled("Error on initialize DB");
			}
		}
	}
}
