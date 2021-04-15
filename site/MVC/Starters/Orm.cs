using System;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using DfM.Logs;
using DFM.MVC.Helpers;
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

				app.Use<Orm>(async (context, next) =>
				{
					var path = context.Request.Path.Value;

					if (path != null && path.StartsWith("/Assets"))
						return;

					SessionManager.Init(
						() => Session.GetKey(() => context)
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
