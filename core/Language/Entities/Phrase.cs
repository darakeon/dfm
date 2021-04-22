using System;
using System.Text.RegularExpressions;
using DFM.Language.Extensions;

namespace DFM.Language.Entities
{
	public class Phrase : INameable
	{
		public String Name { get; }
		public String Text { get; }

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
