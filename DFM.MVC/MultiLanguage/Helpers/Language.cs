using System;
using Ak.DataAccess.XML;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class Language : INameable
    {
        public String Name { get; set; }
        public DicList<Phrase> PhraseList { get; set; }

        public Language()
        {
            PhraseList = new DicList<Phrase>();
        }

        public Language(Node nodeLanguage) : this()
        {
            Name = nodeLanguage.Name;

            foreach (var nodePhrase in nodeLanguage)
            {
                var dicPhrase = new Phrase(nodePhrase);

                PhraseList.Add(dicPhrase);
            }
        }

        public Phrase this[String phrase]
        {
            get { return PhraseList[phrase]; }
        }



        public override String ToString()
        {
            return Name;
        }
    }
}
