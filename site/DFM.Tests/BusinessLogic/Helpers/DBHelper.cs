using System;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
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

		internal static String GetLastTokenForUser(String email, SecurityAction action)
		{
			if (FakeHelper.IsFake)
				return FakeRepos.Security.GetLastTokenForUser(email, action);

			var query = @"
				Select Token
					from Security S
						inner join User U
							on S.User_ID = U.ID
					where U.Email = @email
						and S.Action = @action
						and S.Expire >= @expire
				order by S.ID desc, S.Expire desc
					limit 1
			";

			return executeDB(query, cmd =>
			{
				cmd.Parameters.AddWithValue("email", email);
				cmd.Parameters.AddWithValue("action", (Int32) action);
				cmd.Parameters.AddWithValue("expire", DateTime.Now);

				var result = cmd.ExecuteScalar();

				if (result == null)
					throw new DFMRepositoryException("Bad, bad developer. No token for you.");

				return result.ToString();
			});
		}

		internal static String GetLastTicketForUser(String email)
		{
			if (FakeHelper.IsFake)
				return FakeRepos.Ticket.GetLastTicketForUser(email);

			var query = @"
				Select Key_
					from Ticket T
						inner join User U
							on T.User_ID = U.ID
					where U.Email = @email
						and T.Expiration is null
				order by T.ID desc
					limit 1
			";

			return executeDB(query, cmd =>
			{
				cmd.Parameters.AddWithValue("email", email);

				var result = cmd.ExecuteScalar();

				if (result == null)
					throw new DFMRepositoryException("Bad, bad developer. No ticket for you.");

				return result.ToString();
			});
		}

		internal static String GetUserEmailByTicket(String ticket)
		{
			if (FakeHelper.IsFake)
				return FakeRepos.Ticket.GetUserEmailByTicket(ticket);

			var query = @"
				Select U.Email
					from User U
						inner join Ticket T
							on U.ID = T.User_ID
					where T.Key_ = @ticket
						and T.Active = 1
				order by T.ID desc
					limit 1
			";

			return executeDB(query, cmd =>
			{
				cmd.Parameters.AddWithValue("ticket", ticket);

				var result = cmd.ExecuteScalar();

				if (result == null)
					throw new DFMRepositoryException("Bad, bad developer. No e-mail for you.");

				return result.ToString();
			});
		}

		internal static void CreateContract(String contractVersion)
		{
			if (FakeHelper.IsFake)
			{
				FakeRepos.Contract.Create(contractVersion);
				return;
			}

			var query = @"
				INSERT INTO Contract
					(BeginDate, Version)
					VALUES
					(now(), @contractVersion);
			";

			executeDB(query, cmd =>
			{
				cmd.Parameters.AddWithValue("contractVersion", contractVersion);
				cmd.ExecuteNonQuery();
			});
		}

		internal static String GetTFAUser(String email)
		{
			if (FakeHelper.IsFake)
				return FakeRepos.User.GetTFAUser(email);

			var query = @"
				Select TFASecret
					from User
					where Email = @email
			";

			return executeDB(query, cmd =>
			{
				cmd.Parameters.AddWithValue("email", email);

				var result = cmd.ExecuteScalar();

				if (result == null)
					throw new DFMRepositoryException("Bad, bad developer. No two-factor for you.");

				return result.ToString();
			});
		}

		public static Boolean CheckScheduleState(ScheduleInfo schedule)
		{
			if (FakeHelper.IsFake)
				return FakeRepos.Schedule.GetState(schedule.ID);

			var query = @"
				Select Active
					from Schedule
					where ID = @id
			";

			return executeDB(query, cmd =>
			{
				cmd.Parameters.AddWithValue("id", schedule.ID);

				var result = cmd.ExecuteScalar();

				if (result == null)
					throw new DFMRepositoryException("Bad, bad developer. No schedule for you.");

				return result.ToString() == "1";
			});
		}

		private static T executeDB<T>(String query, Func<MySqlCommand, T> action)
		{
			var result = default(T);

			executeDB(
				query,
				cmd => { result = action(cmd); }
			);

			return result;
		}

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
