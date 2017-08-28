using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DK.XML;
using DK.Generic.Extensions;
using DFM.Multilanguage.Helpers;

namespace DFM.Multilanguage
{
    public class PlainText
    {
        private static String currentPath;

        internal static String MainPath => Path.Combine(currentPath, "MultiLanguage");
	    private static String path => Path.Combine(MainPath, "Resources");

	    public static EmailLayout EmailLayout { get; private set; }
        public static PlainText Dictionary { get; private set; }
        
        public DicList<Section> SectionList { get; private set; }

        private static List<String> acceptedLanguages;

        



        public PlainText()
        {
            SectionList = new DicList<Section>();
            acceptedLanguages = new List<String>();
        }

        private PlainText(IList<String> xmls) : this()
        {
            var nodes = xmls.Select(x => new Node(x)).ToList();

            check(xmls, nodes);

            nodes.ForEach(addSectionToDictionary);
        }

        private static void check(IList<String> xmls, IList<Node> nodes)
        {
            for (var i = 0; i < xmls.Count; i++)
            {
                var fileName = new FileInfo(xmls[i])
                    .Name.Replace(".xml", "").ToLower();

                if (fileName != nodes[i].Name)
                    throw new Exception(
                        String.Format("File: {0}; Node: {1}", fileName, nodes[i].Name));
            }
        }

        private void addSectionToDictionary(Node nodeSection)
        {
            var dicSection = new Section(nodeSection);

            SectionList.Add(dicSection);
        }




        public static void Initialize(String currentXmlPath)
        {
            currentPath = currentXmlPath;

            Dictionary = new PlainText(Directory.GetFiles(path, "*.xml"));
            EmailLayout = new EmailLayout();

            setAcceptedLaguages();
        }

        private static void setAcceptedLaguages()
        {
            var list = new List<String>();

            Dictionary.SectionList.ForEach(
                s => list.AddRange( 
                        s.LanguageList.Select(l => l.Name.ToLower())
                    )
                );

            acceptedLanguages = list.Distinct().ToList();
        }

        public static Boolean AcceptLanguage(String chosenLanguage)
        {
            return acceptedLanguages.Contains(chosenLanguage.ToLower());
        }

        public static IList<String> AcceptedLanguage()
        {
            return acceptedLanguages.ToList();
        }



        public String this[String section, String language, params String[] phrase]
        {
            get
            {
                if (phrase.Length == 0)
                    throw new ArgumentException("Need at least one phrase.");

                var entire = String.Empty;

                phrase.ToList().ForEach(p => entire += this[section, language, p] + " ");
                
                return entire.Trim();
            }
        }

        private String this[String section, String language, String phrase]
        {
            get
            {
                phrase = Phrase.RemoveWrongCharacters(phrase);

                return tryGetText("general", language, phrase)
                    ?? tryGetText(section, language, phrase)
                    ?? tryGetText("error", language, phrase)
                    ?? tryGetText("email", language, phrase)
                    ?? notFound(section, language, phrase);
            }
        }

        private String tryGetText(String chosenSection, String language, String phrase)
        {
            try { return SectionList[chosenSection][language][phrase].Text; }
            catch (DicException) { return null; }
        }

        private static String notFound(String section, String language, String phrase)
        {
            throw new DicException(String.Format("S: {0} /// L: {1} /// P: {2}", section, language, phrase));
        }
        


        public static String GetMonthName(Int32 month, String language)
        {
            return CultureInfo.GetCultureInfo(language)
                .DateTimeFormat.GetMonthName(month).Capitalize();
        }

        public static NumberFormatInfo GetNumberFormat(String language)
        {
            return CultureInfo.GetCultureInfo(language).NumberFormat;
        }

    
    }
}