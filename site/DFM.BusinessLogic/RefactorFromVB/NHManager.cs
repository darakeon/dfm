using System;
using System.Collections.Generic;
using NHibernate;
using VB.Entities;
using VB.Entities.Base;
using VB.Generics.Cookies;
using VB.DBManager.Mappings;
using VB.DBManager.NHConfig;
using VB.DBManager.NHConfig.UserPassed;

namespace VB.DBManager
{
    public class NHManager
    {
        private static readonly IDictionary<String, SessionWithCount> sessionList = new Dictionary<String, SessionWithCount>();

        public static ISession Session
        {
            get { return getSession(key).NHSession; }
        }

        public static ISession SessionOld
        {
            get { return getSession(keyOld).NHSession; }
        }

        private static SessionWithCount getSession(String sessionKey)
        {
            if (!sessionList.ContainsKey(sessionKey))
            {
                var session = SessionBuilder.Open();

                try
                {
                    sessionList.Add(sessionKey, new SessionWithCount(session));
                }
                catch (ArgumentException) { }
            }

            return sessionList[sessionKey];
        }


        public static void Open()
        {
            getSession(key).AddUse();
            getSession(keyOld).AddUse();
        }


        public static void Start()
        {
            var mapInfo = 
                new AutoMappingInfo<StartupMap, STARTUP>
                {
                    EntityBase = typeof (IEntity)
                };

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

        private static void close(Boolean isActive, String sessionKey)
        {
            if (!isActive)
                return;

            var session = getSession(sessionKey);

            session.RemoveUse();

            if (!session.IsInUse())
            {
                sessionList.Remove(sessionKey);
            }
        }



        public static void End()
        {
            SessionBuilder.End();
        }



        private static String key
        {
            get { return MyCookie.Get(); }
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
                return getSession(sessionKey).NHSession.IsOpen;
            }
            catch (Exception)
            {
                return false;
            }
        }




        internal class SessionWithCount
        {
            public SessionWithCount(ISession session)
            {
                NHSession = session;
            }

            public void AddUse()
            {
                Count++;
            }

            public void RemoveUse()
            {
                Count--;
            }

            public Boolean IsInUse()
            {
                return Count > 0;
            }

            public ISession NHSession { get; private set; }
            public Int32 Count { get; private set; }

        }


    }
}
