using System;
using System.Collections.Generic;
using System.Web;
using DFM.Email;

namespace DFM.MVC.Helpers
{
    public class ErrorManager
    {
        public static void SendEmail()
        {
            EmailSent = Error.SendReport(HttpContext.Current.AllErrors);

        }

        /// <summary>
        /// When its value is gotten, its emptied
        /// </summary>
        public static Error.Status EmailSent
        {
            get
            {
                if (!errors.ContainsKey(key))
                    return Error.Status.Empty;

                var result = errors[key];

                errors[key] = Error.Status.Empty;

                return result;
            }
            private set
            {
                if (!errors.ContainsKey(key))
                    errors.Add(key, value);
                else
                    errors[key] = value;
            }
        }



        private static readonly 
            IDictionary<String, Error.Status> errors 
                = new Dictionary<String, Error.Status>();


        private static String key
        {
            get
            {
                // ReSharper disable PossibleNullReferenceException
                return HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value;
                // ReSharper restore PossibleNullReferenceException
            }
        }



    }
}