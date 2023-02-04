using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Generic;
using DFM.Tests.Util;
using Keon.Util.Crypto;
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
			using var csv = new CSV();

			csv.Add(moves);
			csv.Add(schedules);

			var email = $"{scenarioCode}@dontflymoney.com";
			var hashedEmail = Crypt.Do(email);
			csv.Create(new Wipe { HashedEmail = hashedEmail });

			var filename = Directory
				.GetFiles(
					Directory.GetCurrentDirectory(),
					$"{hashedEmail.ToBase64()}*.csv"
				)
				.OrderByDescending(s => s)
				.FirstOrDefault();

			if (filename != null)
				this.csv = File.ReadAllLines(filename);
		}

		[Then(@"the file will have these lines")]
		public void ThenTheFileWillHaveTheseLines(Table table)
		{
			var expected = table.Rows.Select(r => r["File"]);

			Assert.AreEqual(expected, csv);
		}

		[Then(@"there will be no file generation")]
		public void ThenThereWillBeNoFileGeneration()
		{
			Assert.Null(csv);
		}
	}
}
