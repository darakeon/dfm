using System;
using System.Configuration;
using System.IO;
using FluentNHibernate.Automapping.Alterations;
using NHibernate;
using VB.Generics.Enums;
using VB.DBManager.NHConfig.Helpers;
using VB.DBManager.NHConfig.UserPassed;

namespace VB.DBManager.NHConfig
{
    /// <summary>
    /// To create NHibernate Session and communicate with DB
    /// </summary>
    public class SessionBuilder
    {
        ///<summary>
        /// Create Session Factory.
        /// To be used at Application_Start.
        ///</summary>
        /// <typeparam name="TM">Any mapping class. Passed automatic by passing to AutoMappingInfo.</typeparam>
        /// <typeparam name="TE">Any entity class. Passed automatic by passing to AutoMappingInfo.</typeparam>
        /// <param name="connectionInfo">About database connection</param>
        /// <param name="autoMappingInfo">About mappings on the entities</param>
        public static void Start<TM, TE>(ConnectionInfo connectionInfo, AutoMappingInfo<TM, TE> autoMappingInfo)
            where TM : IAutoMappingOverride<TE>
        {
            if (connectionInfo == null)
            {
                var scriptFileFullName = ConfigurationManager.AppSettings["ScriptFileFullName"];

                if (scriptFileFullName != null 
                    && scriptFileFullName.ToLower() == "current")
                {
                    scriptFileFullName = Directory.GetCurrentDirectory();
                    var filename = DateTime.Now.ToString(@"Ba\seyyyyMMddhhmmss");

                    scriptFileFullName = Path.Combine(scriptFileFullName, filename);
                }

                connectionInfo = new ConnectionInfo
                {
                    DBMS = Str2Enum.Cast<DBMS>(ConfigurationManager.AppSettings["DBMS"]),
                    DBAction = Str2Enum.Cast<DBAction>(ConfigurationManager.AppSettings["DBAction"] ?? "None"),
                    ScriptFileFullName = scriptFileFullName,
                    ShowSQL = (ConfigurationManager.AppSettings["ShowSQL"] ?? "false").ToLower() == "true",
                    ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]
                };
            }

            SessionFactoryBuilder.CreateSessionFactory(connectionInfo, autoMappingInfo);
        }



        /// <summary>
        /// Create Session Factory, using the AppSettings.
        /// The keys required are the ConnectionInfo class properties.
        /// To be used at Application_Start.
        /// </summary>
        /// <typeparam name="TM">Any mapping class. Passed automatic by passing to AutoMappingInfo.</typeparam>
        /// <typeparam name="TE">Any entity class. Passed automatic by passing to AutoMappingInfo.</typeparam>
        /// <param name="autoMappingInfo">About mappings on the entities</param>
        public static void Start<TM, TE>(AutoMappingInfo<TM, TE> autoMappingInfo)
            where TM : IAutoMappingOverride<TE>
        {
            Start(null, autoMappingInfo);
        }



        /// <summary>
        /// Open the NH session.
        /// To be used at Application_BeginRequest.
        /// </summary>
        public static ISession Open()
        {
            return SessionFactoryBuilder.OpenSession();
        }

        
        
        ///<summary>
        /// Close the SessionFactory.
        /// To be used at Application_End.
        ///</summary>
        public static void End()
        {
            SessionFactoryBuilder.End();
        }



        ///<summary>
        /// Rollback the actions in case of Error.
        /// To be used at Application_Error.
        ///</summary>
        public static void Error(ISession session)
        {
            session.Clear();
        }



        ///<summary>
        /// Force the Initialize of NHibernate objects.
        ///</summary>
        public static void NhInitialize(ISession session, object obj)
        {
            if (!session.Contains(obj))
                NHibernateUtil.Initialize(obj);
        }
    }
}