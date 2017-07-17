using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Ak.DataAccess.XML;
using Ak.MVC.Route;
using DFM.MVC.Helpers;
using DFM.MVC.MultiLanguage.Helpers;
using NHibernate.Linq;

namespace DFM.MVC.MultiLanguage
{
    public class PlainText
    {
        public static PlainText Dictionary;
        public static List<String> AcceptedLanguages;
        private static readonly String path = HttpContext.Current.Server.MapPath(@"~\MultiLanguage\Resources");

        public DicList<Section> SectionList { get; private set; }



        public PlainText()
        {
            SectionList = new DicList<Section>();
            AcceptedLanguages = new List<String>();
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
                        s.LanguageList.Select(l => l.Name)
                    )
                );

            AcceptedLanguages = list.Distinct().ToList();
        }



        public static CultureInfo Culture { get { return Thread.CurrentThread.CurrentUICulture; } }

        private static String section { get { return RouteInfo.Current.RouteData.Values["controller"].ToString().ToLower(); } }

        private static String language { get { return Culture.Name; } }



        public String this[object phrase]
        {
            get { return this[phrase.ToString()]; }
        }

        public String this[String phrase]
        {
            get
            {
                try { return SectionList["general"][language][phrase].Text; }
                catch (DicException)
                {
                    try { return SectionList[section][language][phrase].Text; }
                    catch (DicException)
                    {
                        phrase = Phrase.RemoveWrongCharacters(phrase);

                        DictionaryCreator.Fix(path, section, language, phrase);
                        
                        return Dictionary[phrase];
                    }
                }
            }
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

        

        public static Dictionary<TEnum, String> GetEnumNames<TEnum>()
        {
            var natures = new Dictionary<TEnum, String>();

            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                var key = (TEnum)item;
                var value = Dictionary[item];

                natures.Add(key, value);
            }
            return natures;
        }

        public static String GetMonthName(Int32 month)
        {
            return Culture.DateTimeFormat.GetMonthName(month).Capitalize();
        }
    }
}