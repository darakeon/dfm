using System;
using System.Collections.Generic;
using DFM.API.Starters.Routes;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.API.Tests
{
	[Binding]
	public class RouteStep
	{
		private readonly IDictionary<String, Url> urls = new Dictionary<String, Url>();
		private readonly Queue<String> results = new();

		[Given(@"I have these patterns")]
		public void GivenIHaveThesePatterns(Table table)
		{
			foreach (var row in table.Rows)
			{
				urls.Add(row["Name"], new Url(row["Pattern"]));
			}
		}

		[When(@"I ask the route for these values")]
		public void WhenIAskTheRouteForTheseValues(Table table)
		{
			foreach (var row in table.Rows)
			{
				var url = urls[row["Name"]];

				var result = url.Translate(new
				{
					controller = row["Controller"],
					action = row["Action"],
					id = row["ID"],
				});

				results.Enqueue(result);
			}
		}

		[Then(@"my routes would be")]
		public void ThenMyRoutesWouldBe(Table table)
		{
			foreach (var row in table.Rows)
			{
				var expected = row["Route"];
				var actual = results.Dequeue();

				Assert.That(actual, Is.EqualTo(expected));
			}

			Assert.That(results, Is.Empty);
		}
	}
}
