using System;
using System.Collections.Generic;
using System.IO;
using Ak.MVC.Route;
using DFM.Multilanguage;
using DFM.Multilanguage.Helpers;
using System.Web;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Helpers
{
    public class MultiLanguage
    {
        public static void Initialize()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "bin");

            PlainText.Initialize(path);
        }


        private static MultiLanguage dictionary;

        public static MultiLanguage Dictionary
        {
            get
            {
                return dictionary 
                    ?? (dictionary = new MultiLanguage());
            }
        }


        public String this[DFMCoreException exception]
        {
            get { return this[exception.Type.ToString()]; }
        }

        public String this[ExceptionPossibilities exception]
        {
            get { return this[exception.ToString()]; }
        }

        public String this[params String[] phrase]
        {
            get { return PlainText.Dictionary[section, Language, phrase]; }
        }


        public static String EmailLayout(String layout)
        {
            return PlainText.EmailLayout[Language, layout];
        }

        public static String GetMonthName(Int32 month)
        {
            return PlainText.GetMonthName(month, Language);
        }



        private static String section
        {
            get
            {
                var current = RouteInfo.Current;

                if (current == null || current.RouteData == null)
                    return "Ops";

                var controller = current.RouteData.Values["controller"].ToString().ToLower();

                if (controller.StartsWith("?"))
                {
                    var defaults = ((System.Web.Routing.Route) current.RouteData.Route).Defaults;

                    controller = defaults["controller"].ToString().ToLower();
                }

                return controller;
            }
        }

        public static String Language
        {
            get
            {
                var browserLanguage =
                    request.UserLanguages != null && request.UserLanguages.Length > 0
                        ? request.UserLanguages[0]
                        : null;

                var userLanguage = Auth.Current.Language ?? browserLanguage;

                if (userLanguage == null || !PlainText.AcceptLanguage(userLanguage))
                    userLanguage = "en-US";

                return userLanguage;
            }
        }



        private static HttpRequest request
        {
            get { return HttpContext.Current.Request; }
        }


        public static IDictionary<T, String> GetEnumNames<T>()
        {
            return EnumHelper.GetEnumNames<T>(section, Language);
        }


    }
}