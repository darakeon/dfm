using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Language.Extensions;

namespace DFM.Language.Entities
{
	public class Language : INameable
	{
		public String Name { get; }
		public DicList<Phrase> PhraseList { get; }

		internal Language(String name, IDictionary<String, Object> sentences)
		{
			PhraseList = new DicList<Phrase>();

			Name = name.Replace("_", "-");

			var validSentences = sentences
				.Where(s => !String.IsNullOrEmpty(s.Key));

			foreach (var (key, value) in validSentences)
			{
				if (String.IsNullOrEmpty(key))
					continue;

				var dicPhrase = new Phrase(key, value);
				PhraseList.Add(dicPhrase);
			}
		}

		public Phrase this[String phrase] => PhraseList[phrase];

		public override String ToString()
		{
			return Name;
		}
	}
}
