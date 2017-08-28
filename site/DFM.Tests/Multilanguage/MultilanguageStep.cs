using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFM.Multilanguage.Helpers;
using DFM.Tests.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using DFM.Multilanguage;

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


		[Given(@"I have these keys")]
		public void GivenIHaveTheseKeys(Table table)
		{
			keys = table.Rows
				.Select(r => new Triad(r["Section"], r["Phrase"]))
				.ToList();
		}


		[Given(@"I have these e-mail types")]
		public void GivenIHaveTheseEmailTypes(Table table)
		{
			emailTypes = table.Rows
				.Select(r => r["Phrase"])
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
			foreach (var language in languages)
			{
				foreach (var emailType in emailTypes)
				{
					try
					{
						var layout = PlainText.EmailLayout[language, emailType];

						if (String.IsNullOrEmpty(layout))
							errors.AppendLine($"Null at L: {language}, T: {emailType}");
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

		private static IList<Triad> keys
		{
			get { return Get<IList<Triad>>("keys"); }
			set { Set("keys", value); }
		}

		private static IList<String> emailTypes
		{
			get { return Get<IList<String>>("emailTypes"); }
			set { Set("emailTypes", value); }
		}



		class Triad
		{
			public Triad(String section, String phrase)
			{
				Section = section;
				Phrase = phrase;
			}

			public String Section { get; }
			public String Phrase { get; }
		}

	}
}
