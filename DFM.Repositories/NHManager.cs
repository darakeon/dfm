using System;
using System.Collections.Generic;
using Ak.DataAccess.NHibernate;
using Ak.DataAccess.NHibernate.UserPassed;
using Ak.Generic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Repositories.Mappings;
using NHibernate;
using DFM.Generic;

namespace DFM.Repositories
{
    public class NHManager
    {
        private static readonly IDictionary<String, ISession> sessionList = new Dictionary<String, ISession>();

        public static ISession Session
        {
            get
            {
                if (!sessionList.ContainsKey(key))
                {
                    var session = SessionBuilder.Open();

                    if (!sessionList.ContainsKey(key))
                        sessionList.Add(key, session);
                }

                return sessionList[key];
            }
        }



        public static void Start()
        {
            var mapInfo = new AutoMappingInfo<UserMap, User>
                              {
                                  EntityBase = typeof (BaseMove)
                              };

            SessionBuilder.Start(mapInfo);
        }

        public static void Error()
        {
            SessionBuilder.Error(Session);

            sessionList.Remove(key);
        }

        public static void Close()
        {
            if (!IsActive)
                return;

            SessionBuilder.Close(Session);

            sessionList.Remove(key);
        }

        public static void End()
        {
            SessionBuilder.End();
        }



        private static String key
        {
            get { return Identity.GetGeneratedKeyFor("NHManager"); }
        }



        public static Boolean IsActive
        {
            get
            {
                try
                {
                    return Session.IsOpen;
                }
                catch (AkException)
                {
                    return false;
                }                
            }
        }


    }
}
