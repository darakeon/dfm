using System;

namespace DFM.Multilanguage.Helpers
{
	public class Section : INameable
	{
		public String Name { get; set; }
		public DicList<Language> LanguageList { get; set; }

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
