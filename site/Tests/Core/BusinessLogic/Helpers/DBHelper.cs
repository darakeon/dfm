using System;
using DFM.Generic;
using Keon.NHibernate.Fakes;
using MySql.Data.MySqlClient;

namespace DFM.Tests.BusinessLogic.Helpers
{
	internal class DBHelper
	{
		internal static void Cleanup()
		{
			if (FakeHelper.IsFake)
				return;

			var query = @"
				set sql_safe_updates = 0;

				delete from detail;
				delete from move;
				delete from schedule;
				delete from summary;
				delete from category;
				delete from month;
				delete from year;
				delete from account;

				delete from ticket;
				delete from security;
				delete from acceptance;
				delete from contract;
				delete from user;
				delete from config;

				set sql_safe_updates = 1;
			";

			executeDB(query, cmd => { cmd.ExecuteNonQuery(); });
		}

		static readonly String connStr =
			$"Server={Cfg.Server};Database={Cfg.DataBase};Uid={Cfg.Login};Pwd={Cfg.Password};";

		private static void executeDB(String query, Action<MySqlCommand> action)
		{
			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				try
				{
					using (var cmd = new MySqlCommand(query, conn))
					{
						action(cmd);
					}
				}
				finally
				{
					conn.Close();
				}
			}
		}
	}
}
