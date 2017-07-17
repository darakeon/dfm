using System;
using Ak.DataAccess.XML;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class Phrase : INameable
    {
        public String Name { get; set; }
        public String Text { get; set; }

        public Phrase() { }

        public Phrase(Node nodePhrase) : this()
        {
            Name = nodePhrase.Name;
            Text = nodePhrase["text"];
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
