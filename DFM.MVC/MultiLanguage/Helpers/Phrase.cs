using System;
using System.Text.RegularExpressions;
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

        
        public static String RemoveWrongCharacters(String phrase)
        {
            var regex = new Regex(@"[^a-zA-Z0-9]*");

            return regex.Replace(phrase, "");
        }
    }
}
