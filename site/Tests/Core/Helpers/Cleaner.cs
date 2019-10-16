using System;
using DFM.Generic;
using Keon.NHibernate.Fakes;
using MySql.Data.MySqlClient;

namespace DFM.Tests.Helpers
{
	public class Cleaner
	{
		public static void Cleanup()
		{
			if (FakeHelper.IsFake)
				return;

			execute("call cleanup");
		}

		static readonly String connStr =
			$"Server={Cfg.Server};" +
			$"Database={Cfg.DataBase};" +
			$"Uid={Cfg.Login};" +
			$"Pwd={Cfg.Password};" +
			"Connect Timeout=10000";

		private static void execute(String query)
		{
			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				try
				{
					using (var cmd = new MySqlCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
					}
				}
				finally
				{
					conn.Close();
					conn.Dispose();
				}
			}
		}
	}
}
