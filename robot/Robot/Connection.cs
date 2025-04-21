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
		private const String envVarName = "ASPNETCORE_ENVIRONMENT";
		private const String sessionName = "ROBOT";

		private static readonly String name = 
			Environment.GetEnvironmentVariable(envVarName);

		public static async Task Run(Func<Task> action)
		{
			Cfg.Init(name);
			PlainText.Initialize();

			try
			{
				await sessionFactory(() => session(action));
			}
			catch (Exception e)
			{
				LogFactory.Service.Log(e);
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
