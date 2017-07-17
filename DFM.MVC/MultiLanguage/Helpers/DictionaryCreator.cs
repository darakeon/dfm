using System;
using System.IO;
using System.Linq;
using Ak.DataAccess.XML;

namespace DFM.MVC.MultiLanguage.Helpers
{
    class DictionaryCreator
    {
        public static void Fix(String path, String section, String language, String phrase)
        {
            var sectionFile = getSection(path, section);

            var languageNode = getLanguage(sectionFile, language);

            getPhrase(languageNode, phrase);

            sectionFile.Overwrite();

            PlainText.Initialize();
        }



        private static Node getSection(String path, String section)
        {
            var fileName = Path.Combine(path, section + ".xml");

            if (!File.Exists(fileName))
            {
                createSection(section, fileName);
            }

            return new Node(fileName);
        }

        private static void createSection(string section, string fileName)
        {
            var file = File.Create(fileName);

            var parentNode = String.Format(
                        "<?xml version='1.0' encoding='utf-8' ?><{0}></{0}>", section)
                    .Select(c => (byte)c).ToArray();

            file.Write(parentNode, 0, parentNode.Length);

            file.Flush(); file.Close();
        }



        private static Node getLanguage(Node sectionFile, String language)
        {
            var languageNode = sectionFile.Childs.SingleOrDefault(c => c.Name == language.ToString());

            if (languageNode == null)
                languageNode = createLanguage(sectionFile, language);

            return languageNode;
        }

        private static Node createLanguage(Node sectionFile, String language)
        {
            var languageNode = new Node { Name = language };
            
            sectionFile.Add(languageNode);
            
            return languageNode;
        }



        private static void getPhrase(Node languageNode, String phrase)
        {
            var phraseNode = languageNode.Childs.SingleOrDefault(c => c.Name == phrase);

            if (phraseNode == null)
                createPhrase(languageNode, phrase);
        }

        private static void createPhrase(Node languageNode, string phrase)
        {
            var phraseNode = new Node { Name = phrase };

            phraseNode.Add("text", phrase);
            phraseNode.Add("automatic", null);
            
            languageNode.Add(phraseNode);
        }

    }
}
