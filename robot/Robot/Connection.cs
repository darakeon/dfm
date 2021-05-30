using System;
using DFM.BusinessLogic.Repositories.Mappings;
using DFM.Entities;
using DFM.Generic;
using DFM.Language;
using Keon.NHibernate.Schema;
using Keon.NHibernate.Sessions;

namespace DFM.Robot
{
	class Connection
	{
		private const String envVarName = "ASPNETCORE_ENVIRONMENT";
		private const String sessionName = "ROBOT";

		private static readonly String name = 
			Environment.GetEnvironmentVariable(envVarName);

		public static void Run(Action action)
		{
			Cfg.Init(name);
			PlainText.Initialize();

			sessionFactory(() => session(action));
		}

		private static void sessionFactory(Action action)
		{
			SessionFactoryManager.Initialize<UserMap, User>(Cfg.DB);

			try
			{
				action();
			}
			finally
			{
				SessionFactoryManager.End();
			}
		}

		private static void session(Action action)
		{
			SessionManager.Init(() => sessionName);

			try
			{
				action();
			}
			finally
			{
				SessionManager.Close();
			}
		}

	}
}
