using System;
using System.Collections.Generic;
using System.Security.Principal;
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

        private static String requestCode { get { return HttpContext.Current.Request.GetHashCode().ToString(); } }
        private static IIdentity user { get { return HttpContext.Current.User.Identity; } }
        private static String userKey { get { return user.IsAuthenticated ? user.Name : requestCode; } }

        public static ISession Session
        {
            get
            {
                if (!sessionList.ContainsKey(userKey))
                {
                    var session = SessionBuilder.Open();

                    if (!sessionList.ContainsKey(userKey))
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
