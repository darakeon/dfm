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
            var current = HttpContext.Current;
            var user = current.User.Identity;

            EmailSent = Error.SendReport(current.AllErrors
                , current.Request.Url.ToString()
                , user.IsAuthenticated ? user.Name : "Off");

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
                return HttpContext.Current.Request.Cookies[0].Value;
                // ReSharper restore PossibleNullReferenceException
            }
        }



    }
}