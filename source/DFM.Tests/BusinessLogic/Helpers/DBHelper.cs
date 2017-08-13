using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Repositories;
using MySql.Data.MySqlClient;

namespace DFM.Tests.BusinessLogic.Helpers
{
    internal class DBHelper
    {
        public static String GetLastTokenForUser(String email, String password, SecurityAction action)
        {
            String token;

            var server = ConfigurationManager.AppSettings["Server"];
            var database = ConfigurationManager.AppSettings["DataBase"];
            var login = ConfigurationManager.AppSettings["Login"];
            var passwordDB = ConfigurationManager.AppSettings["Password"];

            var connStr = String.Format("Server={0};Database={1};Uid={2};Pwd={3};", server, database, login, passwordDB);

            using(var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                var query =
                    @"Select Token
                        from Security S
                            inner join User U
                                on S.User_ID = U.ID
                       where U.Email = @email
                         and U.Password = @password
                         and S.Action = @action
                         and S.Active = 1
                         and S.Expire >= @expire
                    order by S.Expire desc
                       limit 1";

                var cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("password", encrypt(password));
                cmd.Parameters.AddWithValue("action", (Int32)action);
                cmd.Parameters.AddWithValue("expire", DateTime.Now);

                var result = cmd.ExecuteScalar();

                if (result == null)
                {
                    conn.Close();

                    throw new DFMRepositoryException("Bad, bad developer. No token for you.");
                }

                token = result.ToString();

                conn.Close();
            }

            return token;
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
