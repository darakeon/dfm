using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using Keon.MVC.Cookies;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Orm
	{
		public static void Set(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
		{
			SessionFactoryManager.Initialize<UserMap, User>(Cfg.DB);

			app.Use(async (context, next) =>
			{
				SessionManager.Init(
					() => BrowserId.Get(() => context)
				);

				await next.Invoke();

				SessionManager.Close();
			});

			life.ApplicationStopping.Register(SessionFactoryManager.End);
		}
	}
}
