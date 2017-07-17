using System;
using System.Web;

namespace DFM.Core.Helpers
{
    public class Dfm
    {
        public static String Url
        {
            get {
                return "http://" +
                    (HttpContext.Current != null 
                    ? HttpContext.Current.Request.Url.Authority
                    : "www.dontflymoney.com");
            }
        }

    }
}
