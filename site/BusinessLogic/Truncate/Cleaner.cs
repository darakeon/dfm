using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Generic;
using Keon.NHibernate.Fakes;
using MySql.Data.MySqlClient;

namespace DFM.BusinessLogic.Truncate
{
	public class Cleaner
	{
		public static void Cleanup()
		{
			if (FakeHelper.IsFake)
				return;

			var entityNames = getOrdered();

			var query = new List<String>
			{
				"set sql_safe_updates=0"
			};

			query.AddRange(
				entityNames.Select(
					n => $"delete from {n}"
				)
			);

			query.Add("set sql_safe_updates=1");

			execute(query);

			addContract();
		}

		private static IList<String> getOrdered()
		{
			var entities = Entities.Get().ToList();
			var result = new List<String>();

			addOrdered(entities, result);

			return result;
		}

		private static void addOrdered(IList<Entities.Entity> entities, List<String> result)
		{
			foreach (var entity in entities)
			{
				if (result.Contains(entity.Name))
					result.Remove(entity.Name);

				result.Add(entity.Name);

				addOrdered(entity.Parents, result);
			}
		}

		static readonly String connStr =
			$"Server={Cfg.Server};" +
			$"Database={Cfg.DataBase};" +
			$"Uid={Cfg.Login};" +
			$"Pwd={Cfg.Password};" +
			"Connect Timeout=10000";

		private static void addContract()
		{
			var contract =
				"insert into contract" +
					" (beginDate, version)" +
					" values" +
					" (now(), 'test')";

			execute(contract);

			var terms =
				"insert into terms" +
					" (contract_ID, language, json)" +
				" select id, 'en-US', '{ \"Text\": \"en-US\" }'" +
					" from contract" +
				" union all" +
				" select id, 'pt-BR', '{ \"Text\": \"pt-BR\" }'" +
					" from contract";

			execute(terms);
		}

		private static void execute(IList<String> queries)
		{
			execute(queries, c => c.ExecuteNonQuery());
		}

		private static void execute(IList<String> queries, Action<MySqlCommand> action)
		{
			queries.ToList()
				.ForEach(q => execute(q, action));
		}

		private static void execute(String query)
		{
			execute(query, c => c.ExecuteNonQuery());
		}

		private static void execute(String query, Action<MySqlCommand> action)
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
					conn.Dispose();
				}
			}
		}
	}
}
