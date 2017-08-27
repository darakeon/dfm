using System;
using FluentNHibernate.Cfg.Db;
using VB.DBManager.NHConfig.Helpers;

namespace VB.DBManager.NHConfig.UserPassed
{
    /// <summary>
    /// Information to Connect to the Database.
    /// </summary>
    public class ConnectionInfo
    {

        /// <summary>
        /// Server Name. No needed for Postgre and SQLite.
        /// </summary>
        public String Server { get; set; }

        /// <summary>
        /// Database name in DBMS. No needed for Oracle and SQLLite.
        /// </summary>
        public String DataBase { get; set; }

        /// <summary>
        /// Login for the DB.
        /// </summary>
        public String Login { get; set; }

        /// <summary>
        /// Password not encrypted for DB.
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Just for SQLite.
        /// </summary>
        public String ConnectionString { get; set; }


        /// <summary>
        /// Database Managment System used.
        /// </summary>
        public DBMS DBMS { get; set; }


        /// <summary>
        /// The Action to be executed when SessionFactory is Created
        /// Old CreateDB.
        /// </summary>
        public DBAction DBAction { get; set; }

        /// <summary>
        /// The File Full Name for export Script.
        /// Just fill if see the script is needed
        /// </summary>
        public String ScriptFileFullName { get; set; }

        /// <summary>
        /// Data to initialize the DB.
        /// </summary>
        public IDataInitializer DataInitializer { get; set; }

        /// <summary>
        /// Whether to show SQL in log
        /// </summary>
        public Boolean ShowSQL { get; set; }


        internal IPersistenceConfigurer ConfigureDataBase()
        {
            switch (DBMS)
            {
                case DBMS.MySQL:
                    var configurerMySQL =
                        ConnectionString == null
                        ? MySQLConfiguration.Standard
                            .ConnectionString(c => c
                                .Server(Server)
                                .Database(DataBase)
                                .Username(Login)
                                .Password(Password))
                        : MySQLConfiguration.Standard
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerMySQL = configurerMySQL.ShowSql();

                    return configurerMySQL;

                case DBMS.MsSql7:
                    var configurerMsSql7 =
                        ConnectionString == null
                        ? MsSqlConfiguration.MsSql7
                            .ConnectionString(c => c
                                .Server(Server)
                                .Database(DataBase)
                                .Username(Login)
                                .Password(Password))
                        : MsSqlConfiguration.MsSql7
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerMsSql7 = configurerMsSql7.ShowSql();

                    return configurerMsSql7;

                case DBMS.MsSql2008:
                    var configurerMsSql2008 =
                        ConnectionString == null
                        ? MsSqlConfiguration.MsSql2008
                            .ConnectionString(c => c
                                .Server(Server)
                                .Database(DataBase)
                                .Username(Login)
                                .Password(Password))
                        : MsSqlConfiguration.MsSql2008
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerMsSql2008 = configurerMsSql2008.ShowSql();

                    return configurerMsSql2008;

                case DBMS.MsSql2005:
                    var configurerMsSql2005 =
                        ConnectionString == null
                        ? MsSqlConfiguration.MsSql2005
                            .ConnectionString(c => c
                                .Server(Server)
                                .Database(DataBase)
                                .Username(Login)
                                .Password(Password))
                        : MsSqlConfiguration.MsSql2005
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerMsSql2005 = configurerMsSql2005.ShowSql();

                    return configurerMsSql2005;

                case DBMS.MsSql2000:
                    var configurerMsSql2000 =
                        ConnectionString == null
                        ? MsSqlConfiguration.MsSql2000
                            .ConnectionString(c => c
                                .Server(Server)
                                .Database(DataBase)
                                .Username(Login)
                                .Password(Password))
                        : MsSqlConfiguration.MsSql2000
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerMsSql2000 = configurerMsSql2000.ShowSql();

                    return configurerMsSql2000;

                case DBMS.Postgre:
                    var configurerPostgre =
                        ConnectionString == null
                        ? PostgreSQLConfiguration.Standard
                            .ConnectionString(c => c
                                .Database(DataBase)
                                .Username(Login)
                                .Password(Password))
                        : PostgreSQLConfiguration.Standard
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerPostgre = configurerPostgre.ShowSql();

                    return configurerPostgre;

                case DBMS.Oracle9:
                    var configurerOracle9 =
                        ConnectionString == null
                        ? OracleClientConfiguration.Oracle9
                            .ConnectionString(c => c
                                .Server(Server)
                                .Username(Login)
                                .Password(Password))
                        : OracleClientConfiguration.Oracle9
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerOracle9 = configurerOracle9.ShowSql();

                    return configurerOracle9;

                case DBMS.Oracle10:
                    var configurerOracle10 =
                        ConnectionString == null
                        ? OracleClientConfiguration.Oracle10
                            .ConnectionString(c => c
                                .Server(Server)
                                .Username(Login)
                                .Password(Password))
                        : OracleClientConfiguration.Oracle10
                            .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerOracle10 = configurerOracle10.ShowSql();

                    return configurerOracle10;

                case DBMS.SQLite:
                    var configurerSQLite = SQLiteConfiguration.Standard
                        .ConnectionString(ConnectionString);

                    if (ShowSQL)
                        configurerSQLite = configurerSQLite.ShowSql();

                    return configurerSQLite;

                default:
                    throw new Exception("Not Suported!");
            }
        }



    }

}