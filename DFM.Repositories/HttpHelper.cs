using System;
using System.Security.Principal;
using System.Web;

namespace DFM.Repositories
{
    internal class HttpHelper
    {
        internal static String UserKey
        {
            get
            {
                return user != null && user.IsAuthenticated 
                    ? user.Name 
                    : requestCode.ToString();
            }
        }


        private static object requestCode
        {
            get
            {
                return HttpContext.Current.Request.GetHashCode();
            }
        }

        
        private static IIdentity user
        {
            get
            {
                return HttpContext.Current.User.Identity;
            }
        }
        
        
    }
}
