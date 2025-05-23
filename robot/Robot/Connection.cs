using System;
using System.Threading.Tasks;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using DFM.Language;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;
using DFM.Logs;

namespace DFM.Robot
{
	class Connection
	{
		private const String sessionName = "ROBOT";

		public static async Task Run(Func<Task> action)
		{
			try
			{
				PlainText.Initialize();

				await sessionFactory(() => session(action));
			}
			catch (Exception e)
			{
				await LogFactory.Service.Log(e);
				throw;
			}
		}

		private static async Task sessionFactory(Func<Task> action)
		{
			SessionFactoryManager.Initialize<UserMap, User>(Cfg.DB);

			try
			{
				await action();
			}
			finally
			{
				SessionFactoryManager.End();
			}
		}

		private static async Task session(Func<Task> action)
		{
			SessionManager.Init(() => sessionName);

			try
			{
				await action();
			}
			finally
			{
				SessionManager.Close();
			}
		}

	}
}
