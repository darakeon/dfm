﻿using System;
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
using TechTalk.SpecFlow.Assist;

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

		[Given(@"I have the phrase ([\w_]+) of (\w+) section")]
		public void GivenIHaveTheKeyOfSection(String phrase, String section)
		{
			this.phrase = phrase;
			this.section = section;
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
						var layout = PlainText.Html[theme, emailType, "any", Misc.Random()];

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

		[When(@"get the list of translations")]
		public void WhenGetTheListOfTranslations()
		{
			translations = new Dictionary<String, Queue<String>>();

			foreach (var language in languages)
			{
				translations.Add(language, new Queue<String>());

				try
				{
					var translation = PlainText.Site[section, language, phrase];

					if (translation == null)
					{
						errors.AppendLine($"Null at S: {section}, L: {language}, P:{phrase}");
						continue;
					}

					translation.Texts.ToList().ForEach(
						translations[language].Enqueue
					);
				}
				catch (DicException e)
				{
					errors.AppendLine(e.Message);
				}
			}
		}



		[Then(@"I will receive no language error")]
		public void ThenIWillReceiveNoLanguageError()
		{
			Assert.That(errors.ToString(), Is.Empty);
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
							var id = $"{section.Name} > {lang} > {phrase.Name}";

							var translation = dic[
								section.Name,
								language,
								phrase.Name
							];

							Assert.That(translation, Is.Not.Null, $"{id} not found");

							var text = translation.Text
								?? String.Join(' ', translation.Texts);

							Assert.That(text, Is.Not.Null, $"{id} not found");
							Assert.That(text, Is.Not.Empty, $"{id} is empty");
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

					Assert.That(distinctPhrases, Is.EqualTo(allPhrases));
				}
			}
		}

		[Then(@"it will return these values")]
		public void ThenItWillReturnTheseValues(Table table)
		{
			var expectedList = table.CreateSet<LangValue>().ToList();

			Assert.That(
				translations.Sum(e => e.Value.Count),
				Is.EqualTo(expectedList.Count)
			);

			foreach (var expected in expectedList)
			{
				var lang = expected.Language;
				var value = expected.Value;

				var translation = translations[lang].Dequeue();
				Assert.That(translation, Is.EqualTo(value));
			}
		}

		[Then(@"all lists should be same size in all languages at (.+) dictionary")]
		public void ThenAllListsShouldBeSameSizeInAllLanguagesAtDictionary(String name)
		{
			var dic = getDic(name);

			foreach (var section in dic.SectionList)
			{
				foreach (var lang in section.LanguageList)
				{
					var multiplePhraseList =
						lang.PhraseList.Where(p => p.Texts != null);

					foreach (var phrase in multiplePhraseList)
					{
						var langToTest = languages
							.Where(l => l != lang.Name);

						foreach (var language in langToTest)
						{
							var id = $"{section.Name} > {lang} > {phrase.Name}";

							var translation = dic[
								section.Name,
								language,
								phrase.Name
							];

							Assert.That(translation, Is.Not.Null, $"{id} not found");

							var texts = translation.Texts;

							Assert.That(texts, Is.Not.Null, $"{id} has no list");
							Assert.That(texts.Count, Is.EqualTo(phrase.Texts.Count), $"{id} has a different size");
						}
					}
				}
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
			get => get<StringBuilder>();
			init => set(value);
		}

		private IList<String> languages
		{
			get => get<IList<String>>();
			set => set(value);
		}

		private IList<Pair> keys
		{
			get => get<IList<Pair>>();
			set => set(value);
		}

		private IList<EmailType> emailTypes
		{
			get => get<IList<EmailType>>();
			set => set(value);
		}

		private IList<Theme> themes
		{
			get => get<IList<Theme>>();
			set => set(value);
		}

		private String phrase
		{
			get => get<String>();
			set => set(value);
		}

		private String section
		{
			get => get<String>();
			set => set(value);
		}

		private IDictionary<String, Queue<String>> translations
		{
			get => get<IDictionary<String, Queue<String>>>();
			set => set(value);
		}

		private class Pair
		{
			public Pair(String section, String phrase)
			{
				Section = section;
				Phrase = phrase;
			}

			public String Section { get; }
			public String Phrase { get; }
		}

		private class LangValue
		{
			public LangValue(String language, String value)
			{
				Language = language;
				Value = value;
			}

			public String Language { get; }
			public String Value { get; }
		}
	}
}
