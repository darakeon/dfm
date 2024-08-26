using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Exchange.Exporter;
using DFM.Generic;
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DFM.Exchange.Tests
{
	[Binding]
	class Step : ContextHelper
	{
		public Step(ScenarioContext context)
			: base(context) { }

		private IEnumerable<TestMove> moves
		{
			get => getList<TestMove>("moves");
			set => set("moves", value);
		}

		private IEnumerable<TestSchedule> schedules
		{
			get => getList<TestSchedule>("schedules");
			set => set("schedules", value);
		}

		private IMove this[String description]
		{
			get
			{
				return (IMove) moves.SingleOrDefault(m => m.Description == description)
					?? schedules.Single(m => m.Description == description);
			}
		}

		private IList<String> csv
		{
			get => get<IList<String>>("csv");
			set => set("csv", value);
		}

		[Given(@"this move data")]
		public void GivenThisMoveData(Table table)
		{
			moves = table.CreateSet<TestMove>();
		}

		[Given(@"this detail data")]
		public void GivenThisDetailData(Table table)
		{
			foreach (var detail in table.CreateSet<TestDetail>())
			{
				this[detail.Parent].DetailList.Add(detail);
			}
		}

		[Given(@"this schedule data")]
		public void GivenThisScheduleData(Table table)
		{
			schedules = table.CreateSet<TestSchedule>();
		}

		[When(@"convert to csv")]
		public void WhenConvertToCsv()
		{
			using var csvExporter = new CSVExporter();

			csvExporter.Add(moves);
			csvExporter.Add(schedules);

			var email = $"{scenarioCode}@dontflymoney.com";
			var user = new User {Email = email, Password = "password"};
			var wipe = Wipe.FromUser(user);

			csvExporter.Create(wipe);

			var filename = Directory
				.GetFiles(
					Directory.GetCurrentDirectory(),
					$"{wipe.HashedEmail.ToBase64()}*.csv"
				)
				.MaxBy(s => s);

			if (filename != null)
				csv = File.ReadAllLines(filename);
		}

		[Then(@"the csv will have these lines")]
		public void ThenTheCsvWillHaveTheseLines(Table table)
		{
			var expected = table.ToCsv();

			Assert.That(csv, Is.EqualTo(expected));
		}

		[Then(@"there will be no file generation")]
		public void ThenThereWillBeNoFileGeneration()
		{
			Assert.That(csv, Is.Null);
		}
	}
}
