using System;
using System.Collections.Generic;
using System.Web;

namespace DFM.Generic
{
    public class Identity
    {
        public static String GetKeyFor(String name)
        {
            return context == null
                ? getKeyInMachine(name)
                : getKeyInCookie(name);
        }
        public static String GetGeneratedKeyFor(String name)
        {
            return context == null
                ? getGeneratedKeyInMachine(name)
                : getGeneratedKeyInCookie(name);
        }

        public static void SetKeyFor(String name, String value)
        {
            if (context == null)
                setKeyInMachine(name, value);
            else
                setKeyInCookie(name, value);
        }

        public static void KillKeyFor(String name)
        {
            if (context == null)
                killKeyInMachine(name);
            else
                killKeyInCookie(name);
        }

        public static Boolean Exists(String name)
        {
            return context == null
                ? existsInMachine(name)
                : existsInCookie(name);
        }



        private static String getKeyInMachine(String name)
        {
            if (!existsInMachine(name))
                throw new DFMException(String.Format("Key [{0}] doesn't exists", name));
                
            return machine[name];
        }

        private static String getGeneratedKeyInMachine(String name)
        {
            if (!existsInMachine(name))
                machine.Add(name, Token.New());

            return getKeyInMachine(name);
        }

        private static void setKeyInMachine(String name, String value)
        {
            if (existsInMachine(name))
                throw new DFMException(String.Format("Key [{0}] already exists", name));

            machine.Add(name, value);
        }

        private static void killKeyInMachine(String name)
        {
            if (existsInMachine(name))
                machine.Remove(name);
        }

        private static Boolean existsInMachine(String name)
        {
            return machine.ContainsKey(name);
        }

        private static readonly IDictionary<String, String> machine = new Dictionary<String, String>();




        private static String getKeyInCookie(String name)
        {
            if (!existsInCookie(name))
                throw new DFMException(String.Format("Key [{0}] doesn't exists", name));

            return cookies[name].Value;
        }

        private static String getGeneratedKeyInCookie(String name)
        {
            if (!existsInCookie(name))
            {
                var value = Token.New();
                var cookie = new HttpCookie(name, value);
                cookies.Add(cookie);
            }

            return getKeyInCookie(name);
        }

        private static void setKeyInCookie(String name, String value)
        {
            if (existsInCookie(name))
                throw new DFMException(String.Format("Key [{0}] already exists", name));

            cookies.Add(new HttpCookie(name, value));
        }

        private static void killKeyInCookie(String name)
        {
            if (existsInCookie(name))
                cookies.Remove(name);
        }

        private static Boolean existsInCookie(String name)
        {
            return cookies[name] != null;
        }

        private static HttpCookieCollection cookies
        {
            get { return context.Request.Cookies; }
        }

        private static HttpContext context
        {
            get { return HttpContext.Current; }
        }



    }
}
