using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Ak.DataAccess.XML;
using Ak.MVC.Route;
using NHibernate.Linq;

namespace DFM.MVC.MultiLanguage
{
    public class PlainText
    {
        public static PlainText Dictionary;
        public static List<String> AcceptedLanguages;
        private static readonly String path = HttpContext.Current.Server.MapPath(@"~\MultiLanguage\Resources");

        private readonly IDictionary<String, IDictionary<String, IDictionary<String, String>>> dictionary;



        private PlainText(IEnumerable<String> xmls)
        {
            dictionary = new Dictionary<String, IDictionary<String, IDictionary<String, String>>>();

            xmls.Select(x => new Node(x))
                .ForEach(addSectionToDictionary);
        }

        private void addSectionToDictionary(Node file)
        {
            var newSection = new Dictionary<String, IDictionary<String, String>>();

            foreach (var newLanguage in file)
            {
                addLanguageToSection(newLanguage, newSection);
            }

            dictionary.Add(file.Name, newSection);
        }

        private static void addLanguageToSection(Node newLanguage, Dictionary<String, IDictionary<String, String>> newSection)
        {
            var phrases = new Dictionary<String, String>();

            foreach (var phrase in newLanguage)
            {
                phrases.Add(phrase.Name, phrase["text"]);
            }

            newSection.Add(newLanguage.Name, phrases);
        }



        public static void Initialize()
        {
            Dictionary = new PlainText(Directory.GetFiles(path, "*.xml"));

            setAcceptedLaguages();
        }

        private static void setAcceptedLaguages()
        {
            AcceptedLanguages = new List<String>();

            Dictionary.dictionary.ForEach(
                s => AcceptedLanguages.AddRange( 
                    s.Value.Select(l => l.Key)
                         )
                );

            AcceptedLanguages = AcceptedLanguages.Distinct().ToList();
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
                try { return dictionary["general"][language][phrase]; }
                catch (KeyNotFoundException)
                {
                    try { return dictionary[section][language][phrase]; }
                    catch (KeyNotFoundException)
                    {
                        if (HttpContext.Current.Request.Url.Host != "localhost")
                            throw notFoundKey(phrase);

                        DictionaryCreator.Fix(path, section, language, phrase);
                        
                        return Dictionary[phrase];
                    }
                }
            }
        }

        private Exception notFoundKey(String phrase)
        {
            return new Exception(
                    String.Format("Key not found (section: {0} / language: {1} / phrase: {2}).", section, language, phrase)
                );
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
            var natures = new Dictionary<TEnum, string>();

            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                var key = (TEnum)item;
                var value = Dictionary[item];

                natures.Add(key, value);
            }
            return natures;
        }
    }
}