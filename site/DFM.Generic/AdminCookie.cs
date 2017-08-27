using System;
using System.Web;

namespace Sulamerica.DomainModel.AutenticacaoAdmin
{
    public static class AdminCookie
    {
        internal static String Get()
        {
            var value = context == null ? local : get();

            return Crypto.Decrypt(value);
        }


        private static String local;



        private const String name = "CookieName";

        private static HttpContext context
        {
            get { return HttpContext.Current; }
        }

        private static HttpCookieCollection requestCookies
        {
            get { return context.Request.Cookies; }
        }

        private static HttpCookieCollection responseCookies
        {
            get { return context.Response.Cookies; }
        }


        private static String get()
        {
            var cookie = requestCookies[name]
                         ?? responseCookies[name];

            if (cookie == null)
                return null;

            if (cookie.Value == null)
                remove();

            return cookie.Value;
        }



        public static void Set(String value)
        {
            remove();

            value = Crypto.Encrypt(value);

            if (context == null)
            {
                local = value;
                return;
            }

            var cookie = new HttpCookie(name)
                             {
                                 Value = value,
                                 Expires = DateTime.Now.AddDays(7)
                             };

            requestCookies.Add(cookie);
            responseCookies.Add(cookie);

            requestCookies[name].Value = value;
            responseCookies[name].Value = value;
        }


        
        public static void Clean()
        {
            remove();
        }



        private static void remove()
        {
            if (context == null)
            {
                local = null;
                return;
            }

            if (requestCookies[name] != null)
                requestCookies[name].Expires =
                    DateTime.Now.AddMilliseconds(-1);

            if (responseCookies[name] != null)
                responseCookies[name].Expires =
                    DateTime.Now.AddMilliseconds(-1);
        }


    }
}