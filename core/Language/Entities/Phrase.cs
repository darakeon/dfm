using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Language.Extensions;
using Newtonsoft.Json.Linq;

namespace DFM.Language.Entities
{
	public class Phrase : INameable
	{
		public String Name { get; }
		public String Text { get; }
		public IList<String> Texts { get; }

		internal Phrase(String name, Object value)
		{
			Name = name;

			switch (value)
			{
				case String text:
					Text = text;
					break;
				case JArray texts:
					Texts = texts
						.Select(t => t.Value<String>())
						.Where(s => !String.IsNullOrEmpty(s))
						.ToList();
					break;
				default:
					throw new NotImplementedException(
						$"Not implemented reader for {value.GetType()}"
					);
			}
		}

		public override String ToString()
		{
			return Text;
		}

		internal static String RemoveWrongCharacters(String phrase)
		{
			var regex = new Regex(@"[^a-zA-Z0-9_]*");

			return regex.Replace(phrase, "");
		}
	}
}
