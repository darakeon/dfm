using System;
using System.Security.Cryptography;
using System.Text;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Repositories;
using MySql.Data.MySqlClient;

namespace DFM.Tests.BusinessLogic.Helpers
{
	internal class DBHelper
	{
		static readonly String server = Cfg.Get("Server");
		static readonly String database = Cfg.Get("DataBase");
		static readonly String login = Cfg.Get("Login");
		static readonly String passwordDB = Cfg.Get("Password");

		static readonly String connStr =
			$"Server={server};Database={database};Uid={login};Pwd={passwordDB};";


		public static String GetLastTokenForUser(String email, String password, SecurityAction action)
		{
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
							and U.Password = @password
							and S.Action = @action
							and S.Expire >= @expire
					order by S.ID desc, S.Expire desc
						limit 1";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("email", email);
					cmd.Parameters.AddWithValue("password", encrypt(password));
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



		internal static String GetLastTicketForUser(String email, String password)
		{
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
							and U.Password = @password
							and T.Expiration is null
					order by T.ID desc
						limit 1";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("email", email);
					cmd.Parameters.AddWithValue("password", encrypt(password));

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

		

		public static String GetUserEmailByTicket(String ticket)
		{
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


		public static void CreateContract(String contractVersion)
		{
			using (var conn = new MySqlConnection(connStr))
			{
				conn.Open();

				var query = @"
					INSERT INTO Contract 
						(BeginDate, Version) 
						VALUES 
						(curdate(), @contractVersion);
				";

				using (var cmd = new MySqlCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("contractVersion", contractVersion);

					cmd.ExecuteNonQuery();
				}

				conn.Close();
			}
		}



		private static String encrypt(String password)
		{
			if (String.IsNullOrEmpty(password))
				return null;

			var md5 = new MD5CryptoServiceProvider();

			var originalBytes = Encoding.Default.GetBytes(password);
			var encodedBytes = md5.ComputeHash(originalBytes);

			var hexCode = BitConverter
							.ToString(encodedBytes)
							.Replace("-", "");

			return hexCode;
		}


	}
}
