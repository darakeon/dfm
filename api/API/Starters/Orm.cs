using System;
using DFM.API.Helpers;
using DFM.API.Helpers.Extensions;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using DfM.Logs;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace DFM.API.Starters
{
	class Orm
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
				e.TryLogHandled("Error on initialize DB");
			}
		}
	}
}
