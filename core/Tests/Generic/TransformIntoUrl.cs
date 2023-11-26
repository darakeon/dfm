using System;
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Generic.Tests
{
	[Binding]
	public sealed class TransformIntoUrl : ContextHelper
	{
		public TransformIntoUrl(ScenarioContext context)
			: base(context) { }

		private String original
		{
			get => get<String>("original");
			set => set("original", value);
		}

		private String transformed
		{
			get => get<String>("transformed");
			set => set("transformed", value);
		}

		[Given(@"the text (.+)")]
		public void GivenTheText(String text)
		{
			original = text;
		}

		[When(@"remove special characters")]
		public void WhenRemoveSpecialCharacters()
		{
			transformed = original.IntoUrl();
		}

		[Then(@"the result will be (.+)")]
		public void ThenTheResultWillBe(String expected)
		{
			Assert.That(transformed, Is.EqualTo(expected));
		}
	}
}
