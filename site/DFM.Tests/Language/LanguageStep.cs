using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.Language;
using DFM.Language.Emails;
using DFM.Language.Extensions;
using DFM.Tests.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.Language
{
	[Binding]
	public class LanguageStep : ContextHelper
	{
		public LanguageStep()
		{
			errors = new StringBuilder();
		}


		[Given(@"the dictionary is initialized")]
		public void TheDictionaryIsInitialized()
		{
			PlainText.Initialize(RunPath);
		}


		[Given(@"I have these languages")]
		public void GivenIHaveTheseLanguages(Table table)
		{
			languages = table.Rows
				.Select(r => r["Language"])
				.ToList();
		}

		[Given(@"I have these entity enums")]
		public void GivenIHaveTheseEntityEnums(Table table)
		{
			keys = new List<Pair>();

			foreach (var row in table.Rows)
			{
				var section = row["Section"];
				var type = row["Enum"];

				var origin = typeof(SecurityAction);
				var assembly = origin.Assembly;
				var @namespace = origin.Namespace;
				var @enum = assembly.GetType(@namespace + "." + type);

				Enum.GetNames(@enum)
					.Select(m => new Pair(section, m))
					.ToList()
					.ForEach(keys.Add);
			}
		}

		[Given(@"I have the error enum")]
		public void GivenIHaveTheErrorEnum()
		{
			keys = Enum.GetNames(typeof(Error))
				.Select(m => new Pair("Error", m))
				.ToList();
		}

		[Given(@"I have these keys")]
		public void GivenIHaveTheseKeys(Table table)
		{
			keys = table.Rows
				.Select(r => new Pair(r["Section"], r["Phrase"]))
				.ToList();
		}


		[Given(@"I have these e-mail types")]
		public void GivenIHaveTheseEmailTypes(Table table)
		{
			emailTypes = table.Rows
				.Select(r => (EmailType)Enum.Parse(typeof(EmailType), r["Phrase"]))
				.ToList();
		}

		[Given(@"I have these themes")]
		public void GivenIHaveTheseThemes(Table table)
		{
			themes = table.Rows
				.Select(r => (SimpleTheme)Enum.Parse(typeof(SimpleTheme), r["Phrase"]))
				.ToList();
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

						if (String.IsNullOrEmpty(translation))
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
						var layoutDark = PlainText.Html[theme, emailType];

						if (String.IsNullOrEmpty(layoutDark))
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
				foreach (var langOrigin in section.LanguageList)
				{
					foreach (var phrase in langOrigin.PhraseList)
					{
						var langToTest = languages
							.Where(l => l != langOrigin.Name);

						foreach (var language in langToTest)
						{
							var text = dic[
								section.Name,
								language,
								phrase.Name
							];

							Assert.IsNotNull(text);
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


		private static StringBuilder errors
		{
			get { return Get<StringBuilder>("errors"); }
			set { Set("errors", value); }
		}

		private static IList<String> languages
		{
			get { return Get<IList<String>>("languages"); }
			set { Set("languages", value); }
		}

		private static IList<Pair> keys
		{
			get { return Get<IList<Pair>>("keys"); }
			set { Set("keys", value); }
		}

		private static IList<EmailType> emailTypes
		{
			get { return Get<IList<EmailType>>("emailTypes"); }
			set { Set("emailTypes", value); }
		}

		private static IList<SimpleTheme> themes
		{
			get { return Get<IList<SimpleTheme>>("themes"); }
			set { Set("themes", value); }
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
