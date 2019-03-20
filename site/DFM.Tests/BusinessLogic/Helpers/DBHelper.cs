using System;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Repositories;
using MySql.Data.MySqlClient;

namespace DFM.Tests.BusinessLogic.Helpers
{
	internal class DBHelper
	{
		static readonly String connStr =
			$"Server={Cfg.Server};Database={Cfg.DataBase};Uid={Cfg.Login};Pwd={Cfg.Password};";

		internal static String GetLastTokenForUser(String email, SecurityAction action)
		{
			if (FakeHelper.IsFake)
			{
				return FakeRepos.Security.GetLastTokenForUser(email, action);
			}

			String token;

			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				var query = @"
					Select Token
						from Security S
							inner join User U
								on S.User_ID = U.ID
						where U.Email = @email
							and S.Action = @action
							and S.Expire >= @expire
					order by S.ID desc, S.Expire desc
						limit 1";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("email", email);
					cmd.Parameters.AddWithValue("action", (Int32) action);
					cmd.Parameters.AddWithValue("expire", DateTime.Now);

					var result = cmd.ExecuteScalar();

					if (result == null)
					{
						conn.Close();

						throw new DFMRepositoryException("Bad, bad developer. No token for you.");
					}

					token = result.ToString();
				}

				conn.Close();
			}

			return token;
		}

		internal static String GetLastTicketForUser(String email)
		{
			if (FakeHelper.IsFake)
			{
				return FakeRepos.Ticket.GetLastTicketForUser(email);
			}

			String ticket;

			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				var query = @"
					Select Key_
						from Ticket T
							inner join User U
								on T.User_ID = U.ID
						where U.Email = @email
							and T.Expiration is null
					order by T.ID desc
						limit 1";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("email", email);

					var result = cmd.ExecuteScalar();

					if (result == null)
					{
						conn.Close();

						throw new DFMRepositoryException("Bad, bad developer. No ticket for you.");
					}

					ticket = result.ToString();
				}

				conn.Close();
			}

			return ticket;
		}

		internal static String GetUserEmailByTicket(String ticket)
		{
			if (FakeHelper.IsFake)
			{
				return FakeRepos.Ticket.GetUserEmailByTicket(ticket);
			}

			String token;

			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				var query = @"
					Select U.Email
						from User U
							inner join Ticket T
								on U.ID = T.User_ID
						where T.Key_ = @ticket
							and T.Active = 1
					order by T.ID desc
						limit 1";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("ticket", ticket);

					var result = cmd.ExecuteScalar();

					if (result == null)
					{
						conn.Close();

						throw new DFMRepositoryException("Bad, bad developer. No e-mail for you.");
					}

					token = result.ToString();
				}

				conn.Close();
			}

			return token;
		}

		internal static void CreateContract(String contractVersion)
		{
			if (FakeHelper.IsFake)
			{
				FakeRepos.Contract.Create(contractVersion);
				return;
			}

			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				var query = @"
					INSERT INTO Contract
						(BeginDate, Version)
						VALUES
						(now(), @contractVersion);
				";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("contractVersion", contractVersion);

					cmd.ExecuteNonQuery();
				}

				conn.Close();
			}
		}

		internal static String GetTFAUser(String email)
		{
			if (FakeHelper.IsFake)
			{
				return FakeRepos.User.GetTFAUser(email);
			}

			String ticket;

			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				var query = @"
					Select TFASecret
						from User
						where Email = @email";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("email", email);

					var result = cmd.ExecuteScalar();

					if (result == null)
					{
						conn.Close();

						throw new DFMRepositoryException("Bad, bad developer. No two-factor for you.");
					}

					ticket = result.ToString();
				}

				conn.Close();
			}

			return ticket;
		}
	}
}
