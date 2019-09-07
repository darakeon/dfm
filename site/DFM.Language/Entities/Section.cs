using System;
using DFM.Language.Extensions;

namespace DFM.Language.Entities
{
	public class Section : INameable
	{
		public String Name { get; }
		public DicList<Language> LanguageList { get; }

		internal Section(String name, JsonDictionary translations)
		{
			LanguageList = new DicList<Language>();

			Name = name;

			foreach (var language in translations)
			{
				var dicLanguage = new Language(
					language.Key,
					language.Value
				);

				LanguageList.Add(dicLanguage);
			}
		}

		public Language this[String language] => LanguageList[language];

		public override String ToString()
		{
			return Name;
		}
	}
}
