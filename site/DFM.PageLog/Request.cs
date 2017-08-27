using System;
using System.Web;
using DFM.Authentication;
using DFM.Entities.Extensions;

namespace DFM.PageLog
{
    class Request
    {
        public String Url { get; private set; }
        public DateTime Date { get; private set; }
        public String User { get; private set; }
        public String IP { get; private set; }

        public Request(HttpContext context, ISafeService safeService)
        {
            var current = new Current(safeService);

            var request = context.Request;

            Url = request.Url.ToString();
            Date = current.IsAuthenticated ? current.User.Now() : DateTime.UtcNow;
            User = current.IsAuthenticated ? current.User.Email : "Off";
            IP = pegarIP(request);
        }


        private String pegarIP(HttpRequest request)
        {
            return request["HTTP_X_FORWARDED_FOR"] ??
                   request["REMOTE_ADDR"] ?? request["LOCAL_ADDR"];
        }


    }
}
