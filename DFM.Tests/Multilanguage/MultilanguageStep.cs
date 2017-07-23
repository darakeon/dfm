using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Multilanguage.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using DFM.Multilanguage;
using System.IO;

namespace DFM.Tests.Multilanguage
{
    [Binding]
    public class MultilanguageStep : ContextHelper
    {
        [Given(@"the dictionary is initialized")]
        public void TheDictionaryIsInitialized()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"..\..\..\DFM.Multilanguage");

            PlainText.Initialize(path);
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
            try
            {
                foreach (var language in languages)
                {
                    foreach (var key in keys)
                    {
                        var translation = PlainText.Dictionary[key.Section, language, key.Phrase];
                    }
                }
            }
            catch (DicException e)
            {
                error = e;
            }
        }



        [When(@"I try get the layout")]
        public void WhenITryGetTheLayout()
        {
            try
            {
                foreach (var language in languages)
                {
                    foreach (var emailType in emailTypes)
                    {
                        var layout = PlainText.EmailLayout[language, emailType];
                    }
                }
            }
            catch (DicException e)
            {
                error = e;
            }
        }



        [Then(@"I will receive no multilanguage error")]
        public void ThenIWillReceiveNoMultilanguageError()
        {
            Assert.IsNull(error);
        }


        private static DicException error
        {
            get { return Get<DicException>("error"); }
            set { Set("error", value); }
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

            public String Section { get; private set; }
            public String Phrase { get; private set; }
        }

    }
}
