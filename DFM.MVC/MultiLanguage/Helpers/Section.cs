using System;
using Ak.DataAccess.XML;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class Section : INameable
    {
        public String Name { get; set; }
        public DicList<Language> LanguageList { get; set; }

        public Section()
        {
            LanguageList = new DicList<Language>();
        }

        public Section(Node nodeSection) : this()
        {
            Name = nodeSection.Name;

            foreach (var nodeLanguage in nodeSection)
            {
                var dicLanguage = new Language(nodeLanguage);

                LanguageList.Add(dicLanguage);
            }
        }

        public Language this[String language]
        {
            get { return LanguageList[language]; }
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
