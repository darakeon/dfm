using System;
using System.Web;
using Ak.MVC.Route;

namespace DFM.Generic
{
    public static class MyCookie
    {
        public static String Get()
        {
            if (context == null)
            {
                return local
                    ?? (local = Token.New());
            }

            var route = new RouteInfo();
            var ticket = route.RouteData.Values["ticket"];

            if (ticket != null)
                return ticket.ToString();

            if (get() == null)
            {
                add(Token.New());
            }

            return get();
        }



        private static String local;


        
        private const String name = "DFM";
        
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



        private static void add(String value)
        {
            remove();

            var cookie = new HttpCookie(name)
            {
                Value = value,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            requestCookies.Add(cookie);
            responseCookies.Add(cookie);

            requestCookies[name].Value = value;
            responseCookies[name].Value = value;
        }

        private static void remove()
        {
            if (requestCookies[name] != null)
                requestCookies[name].Expires =
                    DateTime.UtcNow.AddDays(-1);

            if (responseCookies[name] != null)
                responseCookies[name].Expires =
                    DateTime.UtcNow.AddDays(-1);
        }

        
    }
}
