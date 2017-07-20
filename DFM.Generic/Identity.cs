using System;
using System.Web;

namespace DFM.Generic
{
    public class Identity
    {
        public static String GetCookieGuid(String name)
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
            get { return HttpContext.Current.Request.Cookies; }
        }


    }
}
