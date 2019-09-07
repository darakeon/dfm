using System;
using System.Text.RegularExpressions;

namespace DFM.Multilanguage.Helpers
{
	public class Phrase : INameable
	{
		public String Name { get; set; }
		public String Text { get; set; }

		internal Phrase(String name, String text)
		{
			Name = name;
			Text = text;
		}

		public override String ToString()
		{
			return Name;
		}

		internal static String RemoveWrongCharacters(String phrase)
		{
			var regex = new Regex(@"[^a-zA-Z0-9_]*");

			return regex.Replace(phrase, "");
		}
	}
}
