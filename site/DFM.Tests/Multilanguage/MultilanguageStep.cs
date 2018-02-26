using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.Multilanguage.Helpers;
using DFM.Tests.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using DFM.Multilanguage;
using DFM.Multilanguage.Emails;

namespace DFM.Tests.Multilanguage
{
	[Binding]
	public class MultilanguageStep : ContextHelper
	{
		public MultilanguageStep()
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
			keys = Enum.GetNames(typeof(ExceptionPossibilities))
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
						var translation = PlainText.Dictionary[key.Section, language, key.Phrase];

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
						var layoutDark = PlainText.EmailLayout[theme, emailType];

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



		[Then(@"I will receive no multilanguage error")]
		public void ThenIWillReceiveNoMultilanguageError()
		{
			Assert.IsEmpty(errors.ToString());
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
