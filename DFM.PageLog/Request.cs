using System;
using System.Web;

namespace DFM.PageLog
{
    class Request
    {
        public String Url { get; private set; }
        public DateTime Date { get; private set; }
        public String User { get; private set; }
        public String IP { get; private set; }

        public Request(HttpContext context)
        {
            var user = context.User.Identity;
            var request = context.Request;

            Url = request.Url.ToString();
            Date = DateTime.Now;
            User = user.IsAuthenticated ? user.Name : "Off";
            IP = pegarIP(request);
        }


        private String pegarIP(HttpRequest request)
        {
            return request["HTTP_X_FORWARDED_FOR"] ??
                   request["REMOTE_ADDR"] ?? request["LOCAL_ADDR"];
        }


    }
}
