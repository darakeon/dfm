using System;
using System.Collections.Generic;
using Ak.DataAccess.NHibernate;
using Ak.DataAccess.NHibernate.UserPassed;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Repositories.Mappings;
using NHibernate;

namespace DFM.Repositories
{
    public class NHManager
    {
        private static readonly IDictionary<String, ISession> sessionList = new Dictionary<String, ISession>();

        public static ISession Session
        {
            get
            {
                if (!sessionList.ContainsKey(HttpHelper.UserKey))
                {
                    var session = SessionBuilder.Open();

                    if (!sessionList.ContainsKey(HttpHelper.UserKey))
                        sessionList.Add(HttpHelper.UserKey, session);
                }

                return sessionList[HttpHelper.UserKey];
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

            sessionList.Remove(HttpHelper.UserKey);
        }

        public static void Close()
        {
            SessionBuilder.Close(Session);

            sessionList.Remove(HttpHelper.UserKey);
        }

        public static void End()
        {
            SessionBuilder.End();
        }



    }
}
