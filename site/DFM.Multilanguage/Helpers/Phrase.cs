using System;
using System.Text.RegularExpressions;
using DK.XML;

namespace DFM.Multilanguage.Helpers
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



		public override String ToString()
		{
			return Name;
		}

		
		public static String RemoveWrongCharacters(String phrase)
		{
			var regex = new Regex(@"[^a-zA-Z0-9_]*");

			return regex.Replace(phrase, "");
		}
	}
}
