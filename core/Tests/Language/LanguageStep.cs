using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using DFM.Entities;
using DFM.Generic;
using DFM.Language.Emails;
using DFM.Language.Extensions;
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Language.Tests
{
	[Binding]
	public class LanguageStep : ContextHelper
	{
		public LanguageStep(ScenarioContext context) : base(context)
		{
			errors = new StringBuilder();
		}

		[Given(@"the dictionary is initialized")]
		public void TheDictionaryIsInitialized()
		{
			PlainText.Initialize(runPath);
		}


		[Given(@"I have these languages")]
		public void GivenIHaveTheseLanguages(Table table)
		{
			languages = table.Rows
				.Select(r => r["Language"])
				.ToList();
		}

		[Given(@"I have these enums")]
		public void GivenIHaveTheseEnums(Table table)
		{
			keys = new List<Pair>();

			foreach (var row in table.Rows)
			{
				var section = row["Section"];
				var project = row["Project"];
				var path = row["Path"];
				var type = row["Enum"];

				var @namespace = $"DFM.{project}";
				var assembly = Assembly.LoadFrom($"{@namespace}.dll");

				if (!String.IsNullOrEmpty(path))
					@namespace += $".{path}";

				var fullname = @namespace + "." + type;
				var @enum = assembly.GetType(fullname);

				if (@enum == null)
					throw new InvalidEnumArgumentException(fullname);

				Enum.GetNames(@enum)
					.Select(m => new Pair(section, m))
					.ToList()
					.ForEach(keys.Add);
			}
		}

		[Given(@"I have these keys")]
		public void GivenIHaveTheseKeys(Table table)
		{
			keys = table.Rows
				.Select(r => new Pair(r["Section"], r["Phrase"]))
				.ToList();
		}


		[Given(@"I have the e-mail types")]
		public void GivenIHaveTheseEmailTypes()
		{
			emailTypes = EnumX.AllValues<EmailType>();
		}

		[Given(@"I have the themes")]
		public void GivenIHaveTheseThemes()
		{
			themes = EnumX.AllValues<Theme>();
		}

		[When(@"I try get the translate")]
		public void WhenITryGetTheTranslate()
		{
			foreach (var language in languages)
			{
				foreach (var key in keys)
				{
					try
					{
						var translation = PlainText.Site[key.Section, language, key.Phrase];

						if (translation == null)
							errors.AppendLine($"Null at S: {key.Section}, L: {language}, P:{key.Phrase}");
					}
					catch (DicException e)
					{
						errors.AppendLine(e.Message);
					}
				}
			}
		}



		[When(@"I try get the layout")]
		public void WhenITryGetTheLayout()
		{
			foreach (var emailType in emailTypes)
			{
				foreach (var theme in themes)
				{
					try
					{
						var layout = PlainText.Html[theme, emailType, Misc.Random()];

						if (String.IsNullOrEmpty(layout))
							errors.AppendLine($"Null at {theme} {emailType}");
					}
					catch (DicException e)
					{
						errors.AppendLine(e.Message);
					}
				}
			}
		}



		[Then(@"I will receive no language error")]
		public void ThenIWillReceiveNoLanguageError()
		{
			Assert.IsEmpty(errors.ToString());
		}


		[Then(@"all keys should be available in all languages at (.+) dictionary")]
		public void ThenAllKeysShouldBeAvailableInAllLanguages(String name)
		{
			var dic = getDic(name);

			foreach (var section in dic.SectionList)
			{
				foreach (var lang in section.LanguageList)
				{
					foreach (var phrase in lang.PhraseList)
					{
						var langToTest = languages
							.Where(l => l != lang.Name);

						foreach (var language in langToTest)
						{
							var text = dic[
								section.Name,
								language,
								phrase.Name
							];

							var id = $"{section.Name} > {lang} > {phrase.Name}";
							Assert.IsNotNull(text, $"{id} not found");
							Assert.IsNotEmpty(text, $"{id} is empty");
						}
					}
				}
			}
		}

		[Then(@"no keys should be repeated at (.+) dictionary")]
		public void ThenNoKeysShouldBeRepeated(String name)
		{
			var dic = getDic(name);

			foreach (var section in dic.SectionList)
			{
				foreach (var language in section.LanguageList)
				{
					var allPhrases =
						language.PhraseList.Count;

					var distinctPhrases =
						language.PhraseList
							.Select(p => p.Name)
							.Distinct()
							.Count();

					Assert.AreEqual(allPhrases, distinctPhrases);
				}
			}
		}

		[When(@"count the occurrences of (.+) in (.+)/(.+)")]
		public void WhenCountTheOccurrencesOfIn(String key, String module, String sectionName)
		{
			var dic = getDic(module);
			var section = dic.SectionList.Single(s => s.Name == sectionName);

			translationsCounters = new List<Int32>();
			foreach (var language in section.LanguageList)
			{
				translationsCounters.Add(language.CountTranslations(key));
			}
		}

		[Then(@"it will return (\d+)")]
		public void ThenItWillReturn(Int32 number)
		{
			foreach (var translationsCounter in translationsCounters)
			{
				Assert.AreEqual(number, translationsCounter);
			}
		}

		private PlainText getDic(String name)
		{
			name = name.ToLower();

			switch (name)
			{
				case "site":
					return PlainText.Site;
				case "e-mail":
					return PlainText.Email;
				default:
					throw new NotImplementedException();
			}
		}


		private StringBuilder errors
		{
			get => get<StringBuilder>("errors");
			set => set("errors", value);
		}

		private IList<String> languages
		{
			get => get<IList<String>>("languages");
			set => set("languages", value);
		}

		private IList<Pair> keys
		{
			get => get<IList<Pair>>("keys");
			set => set("keys", value);
		}

		private IList<EmailType> emailTypes
		{
			get => get<IList<EmailType>>("emailTypes");
			set => set("emailTypes", value);
		}

		private IList<Theme> themes
		{
			get => get<IList<Theme>>("themes");
			set => set("themes", value);
		}

		private IList<Int32> translationsCounters
		{
			get => get<IList<Int32>>("translationsCounters");
			set => set("translationsCounters", value);
		}



		class Pair
		{
			public Pair(String section, String phrase)
			{
				Section = section;
				Phrase = phrase;
			}

			public String Section { get; }
			public String Phrase { get; }
		}

	}
}
