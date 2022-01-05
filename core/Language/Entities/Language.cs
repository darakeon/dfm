using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Language.Extensions;

namespace DFM.Language.Entities
{
	public class Language : INameable
	{
		public String Name { get; }
		public DicList<Phrase> PhraseList { get; }

		internal Language(String name, IDictionary<String, String> sentences)
		{
			PhraseList = new DicList<Phrase>();

			Name = name.Replace("_", "-");

			foreach (var sentence in sentences)
			{
				var dicPhrase = new Phrase(
					sentence.Key,
					sentence.Value
				);

				PhraseList.Add(dicPhrase);
			}
		}

		public Phrase this[String phrase] => PhraseList[phrase];

		public override String ToString()
		{
			return Name;
		}

		public Int32 CountTranslations(String phrasePrefix)
		{
			return PhraseList.Count(
				p => Regex.IsMatch(p.Name, $"{phrasePrefix}_\\d+")
			);
		}
	}
}
