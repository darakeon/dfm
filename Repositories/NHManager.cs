using System;
using System.Collections.Generic;
using System.Web;
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
        
        private static HttpRequest request { get { return HttpContext.Current.Request; } }
        private static String userKey { get { return request.UserHostAddress + request.LogonUserIdentity.Name; } }

        public static ISession Session
        {
            get
            {
                if (!sessionList.ContainsKey(userKey))
                {
                    var session = SessionBuilder.Open();
                    sessionList.Add(userKey, session);
                }

                return sessionList[userKey];
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

        public static void Open()
        {
            SessionBuilder.Open();
        }

        public static void Error()
        {
            SessionBuilder.Error(Session);
        }

        public static void Close()
        {
            SessionBuilder.Close(Session);

            sessionList.Remove(userKey);
        }

        public static void End()
        {
            SessionBuilder.End();
        }



    }
}
