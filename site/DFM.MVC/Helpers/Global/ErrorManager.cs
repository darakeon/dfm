using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ak.MVC.Cookies;
using DFM.Email;

namespace DFM.MVC.Helpers.Global
{
    public class ErrorManager
    {
        public static void SendEmail()
        {
            var current = HttpContext.Current;
            var user = current.User.Identity;

            var get = current.Request.QueryString;
            var post = current.Request.Form;

            var getDictionary = get.AllKeys.ToDictionary(k => k, k => get[k]);
            var postDictionary = post.AllKeys.ToDictionary(k => k, k => post[k]);

            Double value;
            var parameters = getDictionary.Union(postDictionary)
                .Where(
                    p => !p.Key.Contains("Password")
                        && !Double.TryParse(p.Value, out value)
                )
                .ToDictionary(p => p.Key, p => p.Value);

            EmailSent = Error.SendReport(current.AllErrors
                , current.Request.Url.ToString()
                , parameters
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
            get { return MyCookie.Get().Key; }
        }


    }
}