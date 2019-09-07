using System;
using System.Collections.Generic;

namespace DFM.Language.Helpers
{
	public class Language : INameable
	{
		public String Name { get; set; }
		public DicList<Phrase> PhraseList { get; set; }

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
	}
}
