using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Ak.DataAccess.XML;
using Ak.Generic.Extensions;
using DFM.Multilanguage.Helpers;

namespace DFM.Multilanguage
{
    public class PlainText
    {
        internal static readonly String MainPath = 
            Path.Combine(Directory.GetCurrentDirectory(), "bin/MultiLanguage");
        
        private static readonly String path = 
            Path.Combine(MainPath, "Resources");

        public static EmailLayout EmailLayout { get; private set; }
        public static PlainText Dictionary { get; private set; }
        
        public DicList<Section> SectionList { get; private set; }

        private static List<String> acceptedLanguages;

        public static CultureInfo Culture { get { return Thread.CurrentThread.CurrentUICulture; } }




        public PlainText()
        {
            SectionList = new DicList<Section>();
            acceptedLanguages = new List<String>();
        }

        private PlainText(IEnumerable<String> xmls) : this()
        {
            var nodes = xmls.Select(x => new Node(x));

            DicCreator.Check(xmls.ToList(), nodes.ToList());

            nodes.ToList().ForEach(addSectionToDictionary);
        }

        private void addSectionToDictionary(Node nodeSection)
        {
            var dicSection = new Section(nodeSection);

            SectionList.Add(dicSection);
        }




        public static void Initialize()
        {
            Dictionary = new PlainText(Directory.GetFiles(path, "*.xml"));
            EmailLayout = new EmailLayout();

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

        public static Boolean AcceptLanguage(String chosenLanguage)
        {
            return acceptedLanguages.Contains(chosenLanguage.ToLower());
        }








        public String this[String section, String language, params String[] phrase]
        {
            get
            {
                if (phrase.Length == 0)
                    throw new ArgumentException("Need at least one phrase.");

                var entire = String.Empty;

                phrase.ToList().ForEach(p => entire += this[section, language, p] + " ");
                
                return entire;
            }
        }

        private String this[String section, String language, String phrase]
        {
            get
            {
                phrase = Phrase.RemoveWrongCharacters(phrase);

                var text = (tryGetText("general", language, phrase)
                    ?? tryGetText(section, language, phrase))
                    ?? tryGetText("error", language, phrase);

                return text ?? notFound(section, language, phrase);
            }
        }

        private String tryGetText(String chosenSection, String language, String phrase)
        {
            try { return SectionList[chosenSection][language][phrase].Text; }
            catch (DicException) { return null; }
        }

        private static String notFound(String section, String language, String phrase)
        {
#if DEBUG
            DicCreator.Fix(path, section, language, phrase);
            return Dictionary[section, language, phrase];
#else
            throw new DicException(String.Format("S: {0}<br/>L: {1}<br/>P: {2}", section, Language, phrase));
#endif
        }
        


        public static String GetMonthName(Int32 month)
        {
            return Culture.DateTimeFormat.GetMonthName(month).Capitalize();
        }




    }
}