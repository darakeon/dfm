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



        private static String getKeyInMachine(String name)
        {
            if (!machine.ContainsKey(name))
                machine.Add(name, Token.New());

            // ReSharper disable PossibleNullReferenceException
            return machine[name];
            // ReSharper restore PossibleNullReferenceException
        }

        private static IDictionary<String, String> machine = 
                   new  Dictionary<String, String>();




        private static String getKeyInCookie(String name)
        {
            if (cookies[name] == null)
            {
                var value = Token.New();
                var cookie = new HttpCookie(name, value);
                cookies.Add(cookie);
            }

            // ReSharper disable PossibleNullReferenceException
            return cookies[name].Value;
            // ReSharper restore PossibleNullReferenceException
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
