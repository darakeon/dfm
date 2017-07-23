using System;
using System.Configuration;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Repositories;
using MySql.Data.MySqlClient;

namespace DFM.Tests.BusinessLogic.Helpers
{
    internal class DBHelper
    {
        public static String GetLastTokenForUser(User user, SecurityAction action)
        {
            String token;

            var server = ConfigurationManager.AppSettings["Server"];
            var database = ConfigurationManager.AppSettings["DataBase"];
            var login = ConfigurationManager.AppSettings["Login"];
            var password = ConfigurationManager.AppSettings["Password"];

            var connStr = String.Format("Server={0};Database={1};Uid={2};Pwd={3};", server, database, login, password);

            using(var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                var query =
                    @"Select Token
                        from Security
                       where User_Id = @user
                         and Action = @action
                         and Active = 1
                         and Expire >= @expire
                    order by Expire desc
                       limit 1";

                var cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("user", user.ID);
                cmd.Parameters.AddWithValue("action", (Int32)action);
                cmd.Parameters.AddWithValue("expire", DateTime.Now);

                var result = cmd.ExecuteScalar();

                if (result == null)
                {
                    conn.Close();

                    throw new DFMRepositoryException(
                        "Bad. bad developer. No token for you. (Don't forget the context next time)");
                }

                token = result.ToString();

                conn.Close();
            }

            return token;
        }

    }
}
