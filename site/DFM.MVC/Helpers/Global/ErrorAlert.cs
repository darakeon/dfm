using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFM.MVC.Helpers.Global
{
    public class ErrorAlert
    {
        private static IList<String> errors
        {
            get
            {
                if (sessionErrors == null)
                    sessionErrors = new List<String>();

                return (List<String>)sessionErrors;
            }
        }

        private static object sessionErrors
        {
            get { return HttpContext.Current.Session["errors"]; }
            set { HttpContext.Current.Session["errors"] = value; }
        }

        public static void Add(String error)
        {
            errors.Add(error);
        }

        public static IList<String> GetAndClean()
        {
            var list = errors
                .Select(e => MultiLanguage.Dictionary[e])
                .ToList();

            errors.Clear();

            return list;
        }

    }
}