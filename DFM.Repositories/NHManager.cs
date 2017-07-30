using System;
using System.Collections.Generic;
using Ak.DataAccess.NHibernate;
using Ak.DataAccess.NHibernate.UserPassed;
using Ak.Generic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Generic.UniqueIdentity;
using DFM.Repositories.Mappings;
using NHibernate;

namespace DFM.Repositories
{
    public class NHManager
    {
        private static readonly IDictionary<String, ISession> sessionList = new Dictionary<String, ISession>();

        public static ISession Session
        {
            get { return getSession(key); }
        }

        public static ISession SessionOld
        {
            get { return getSession(keyOld); }
        }

        private static ISession getSession(String sessionKey)
        {
            if (!sessionList.ContainsKey(sessionKey))
            {
                var session = SessionBuilder.Open();

                try
                {
                    sessionList.Add(sessionKey, session);
                }
                catch (ArgumentException) { }
            }

            return sessionList[sessionKey];
        }


        public static void Start()
        {
            var mapInfo = new AutoMappingInfo<UserMap, User>();

            SessionBuilder.Start(mapInfo);
        }



        public static void Error()
        {
            error(Session, key);
            error(SessionOld, keyOld);
        }

        private static void error(ISession session, string sessionKey)
        {
            SessionBuilder.Error(session);
            sessionList.Remove(sessionKey);
        }



        public static void Close()
        {
            close(IsActive, key);
            close(isActiveOld, keyOld);
        }

        private static void close(bool isActive, string sessionKey)
        {
            if (!isActive)
                return;

            SessionBuilder.Close(getSession(sessionKey));
            sessionList.Remove(sessionKey);
        }



        public static void End()
        {
            SessionBuilder.End();
        }



        private static String key
        {
            get { return Identity.GetGeneratedKeyFor("NHManager"); }
        }

        private static String keyOld
        {
            get { return key + "_old"; }
        }



        public static Boolean IsActive
        {
            get { return isActive(key); }
        }

        private static Boolean isActiveOld
        {
            get { return isActive(keyOld); }
        }

        private static Boolean isActive(String sessionKey)
        {
            try
            {
                return getSession(sessionKey).IsOpen;
            }
            catch (AkException)
            {
                return false;
            }
        }


    }
}
