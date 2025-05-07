using System;
using DFM.BaseWeb.Helpers;
using DFM.BaseWeb.Helpers.Extensions;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using DFM.Logs;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace DFM.BaseWeb.Starters
{
	public class Orm
	{
		public static void Configure(IApplicationBuilder app, IHostApplicationLifetime life)
		{
			try
			{
				SessionFactoryManager.Initialize<UserMap, User>(Cfg.DB);

				app.Use<Orm>(async (context, next) =>
				{
					if (context.IsAsset())
						return;

					var key = Session.GetKey(() => context);
					SessionManager.Init(() => key);

					await next();

					SessionManager.Close();
				});

				life.ApplicationStopping.Register(SessionFactoryManager.End);
			}
			catch (Exception e)
			{
				LogFactory.Service
					.LogHandled(e, "Error on initialize DB");
			}
		}
	}
}
