using System;
using System.Collections.Generic;
using Ak.MVC.Route;
using DFM.Email;
using DFM.Entities.Enums;
using DFM.Multilanguage;
using DFM.Multilanguage.Helpers;
using DFM.MVC.Authentication;
using System.Web;

namespace DFM.MVC.Helpers
{
    public class MultiLanguage
    {
        public static void Initialize()
        {
            PlainText.Initialize();
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

        public String this[params String[] phrase]
        {
            get
            {
                return PlainText.Dictionary[section, Language, phrase];
            }
        }

        public static String EmailLayout(String layout)
        {
            return PlainText.EmailLayout[Language, layout];
        }

        public static String GetMonthName(Int32 month)
        {
            return PlainText.GetMonthName(month);
        }



        private static String section
        {
            get
            {
                return RouteInfo.Current.RouteData
                    .Values["controller"].ToString().ToLower();
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

                var userLanguage = Current.Language ?? browserLanguage;

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

        public static Format GetForMove(MoveNature movenature)
        {
            return EmailFormats.GetForMove(section, Language, movenature);
        }


    }
}