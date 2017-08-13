using System;
using MySql.Data.MySqlClient;
using System.Data;
using CM = System.Configuration.ConfigurationManager;

namespace DFM.PageLog
{
    class Logger
    {
        private static Boolean tableExist;



        public static void Record(Request request)
        {
            using (var conn = getConnection())
            {
                conn.Open();
                
                createTableIfNotExist(conn);
                saveVisit(conn, request);

                conn.Close();
            }
        }



        private static IDbConnection getConnection()
        {
            var connStr = String.Format(
                "Server={0};Database={1};Uid={2};Pwd={3};"
                , CM.AppSettings["Server"]
                , CM.AppSettings["DataBase"]
                , CM.AppSettings["Login"]
                , CM.AppSettings["Password"]);

            return new MySqlConnection(connStr);
        }



        private static void createTableIfNotExist(IDbConnection conn)
        {
            if (tableExist) return;
            
            using (var command = conn.CreateCommand())
            {
                command.CommandText = createCommand;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    if (!e.Message.StartsWith("Table '")
                        || !e.Message.EndsWith("' already exists"))
                        throw;
                }
            }

            tableExist = true;
        }



        private static void saveVisit(IDbConnection conn, Request request)
        {
            using(var command = conn.CreateCommand())
            {
                command.CommandText = insertCommand;

                command.AddParameter("Url", request.Url);
                command.AddParameter("Date", request.Date);
                command.AddParameter("User", request.User);
                command.AddParameter("IP", request.IP);

                command.ExecuteNonQuery();
            }
        }


        private const String insertCommand =
            @"Insert Into PageLog (Url, Date, User, IP)
                        Values (@Url, @Date, @User, @IP)";


        private const String createCommand =
            @"Create Table PageLog (
                ID Int auto_increment Not Null Primary Key,
                Url Varchar(8192) Not Null,
                Date Datetime Not Null,
                User Varchar(100) Not Null,
                IP Varchar(23) Not Null)";


    }
}
