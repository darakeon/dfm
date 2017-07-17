using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ak.DataAccess.XML;
using Ak.MVC.Route;
using DFM.Core.Enums;
using DFM.MVC.Authentication;
using NHibernate.Linq;

namespace DFM.MVC.Resources
{
    public class PlainText
    {
        public static PlainText Dictionary;

        private readonly IDictionary<String, IDictionary<UserLanguage, IDictionary<String, String>>> dictionary;

        private PlainText(IEnumerable<String> xmls)
        {
            dictionary = new Dictionary<String, IDictionary<UserLanguage, IDictionary<String, String>>>();

            xmls.Select(x => new Node(x))
                .ForEach(addSectionToDictionary);
        }

        private void addSectionToDictionary(Node file)
        {
            var section = new Dictionary<UserLanguage, IDictionary<String, String>>();

            foreach (var language in file)
            {
                var phrases = new Dictionary<String, String>();

                foreach (var phrase in language)
                {
                    phrases.Add(phrase.Name, phrase["text"]);
                }

                var lang = (UserLanguage) Enum.Parse(typeof(UserLanguage), language.Name);

                section.Add(lang, phrases);
            }

            dictionary.Add(file.Name, section);
        }



        public static void Initialize()
        {
            var path = HttpContext.Current.Server.MapPath(@"~\Resources");

            Dictionary = new PlainText(Directory.GetFiles(path, "*.xml"));
        }



        public String this[object phrase]
        {
            get { return this[phrase.ToString()]; }
        }

        public String this[String phrase]
        {
            get
            {
                var section = RouteInfo.Current.RouteData
                    .Values["controller"].ToString().ToLower();

                var language = Current.User.Language;

                try { return dictionary["general"][language][phrase]; }
                catch (KeyNotFoundException)
                {
                    try { return dictionary[section][language][phrase]; }
                    catch (KeyNotFoundException) { throw notFoundKey(section, language, phrase); }
                }
            }
        }

        private Exception notFoundKey(string section, UserLanguage language, string phrase)
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