using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Ak.DataAccess.XML;
using Ak.MVC.Route;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.MultiLanguage.Helpers;
using NHibernate.Linq;

namespace DFM.MVC.MultiLanguage
{
    public class PlainText
    {
        public static PlainText Dictionary;
        private static List<String> acceptedLanguages;
        private static readonly String path = HttpContext.Current.Server.MapPath(@"~\MultiLanguage\Resources");

        public DicList<Section> SectionList { get; private set; }



        public PlainText()
        {
            SectionList = new DicList<Section>();
            acceptedLanguages = new List<String>();
        }

        private PlainText(IEnumerable<String> xmls) : this()
        {
            xmls.Select(x => new Node(x))
                .ForEach(addSectionToDictionary);
        }

        private void addSectionToDictionary(Node nodeSection)
        {
            var dicSection = new Section(nodeSection);

            SectionList.Add(dicSection);
        }




        public static void Initialize()
        {
            Dictionary = new PlainText(Directory.GetFiles(path, "*.xml"));

            setAcceptedLaguages();
        }

        private static void setAcceptedLaguages()
        {
            var list = new List<String>();

            Dictionary.SectionList.ForEach(
                s => list.AddRange( 
                        s.LanguageList
                            .Select(l => l.Name.ToLower())
                    )
                );

            acceptedLanguages = list.Distinct().ToList();
        }

        internal static Boolean AcceptLanguage(String chosenLanguage)
        {
            return acceptedLanguages.Contains(chosenLanguage.ToLower());
        }



        public static CultureInfo Culture { get { return Thread.CurrentThread.CurrentUICulture; } }

        private static String section { get { return RouteInfo.Current.RouteData.Values["controller"].ToString().ToLower(); } }

        public static String Language { get { return Culture.Name; } }



        public String this[object phrase]
        {
            get { return this[phrase.ToString()]; }
        }

        public String this[String phrase]
        {
            get
            {
                phrase = Phrase.RemoveWrongCharacters(phrase);

                var text = (tryGetText("general", phrase) 
                    ?? tryGetText(section, phrase))
                    ?? tryGetText("error", phrase);

                return text ?? notFound(phrase);
            }
        }

        private String tryGetText(String chosenSection, String phrase)
        {
            try { return SectionList[chosenSection][Language][phrase].Text; }
            catch (DicException) { return null; }
        }

        private String notFound(String phrase)
        {
            if (HttpContext.Current.Request.Url.Host != "localhost")
                throw new DicException(String.Format("S: {0}<br/>L: {1}<br/>P: {2}", section, Language, phrase));

            DicCreator.Fix(path, section, Language, phrase);
            return Dictionary[phrase];
        }



        public String this[params String[] phrase]
        {
            get
            {
                var entire = String.Empty;

                phrase.ForEach(p => entire += this[p] + " ");
                
                return entire;
            }
        }

        


        public static String GetMonthName(Int32 month)
        {
            return Culture.DateTimeFormat.GetMonthName(month).Capitalize();
        }

    }
}