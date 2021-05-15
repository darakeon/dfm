using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Tests.Util;
using Keon.Util.Extensions;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DFM.Exchange.Tests
{
	[Binding]
	class Step : ContextHelper
	{
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

		private String filename
		{
			get => get<String>("filename");
			set => set("filename", value);
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
			var csv = new CSV();
			csv.Add(moves);
			csv.Add(schedules);

			filename = Token.New();
			csv.Create(new User { Email = filename });
		}

		[Then(@"the file will have these lines")]
		public void ThenTheFileWillHaveTheseLines(Table table)
		{
			var expected = table.Rows.Select(r => r["File"]);
			var actual = File.ReadAllLines($"{filename}.csv");

			Assert.AreEqual(expected, actual);
		}

		[Then(@"there will be no file")]
		public void ThenThereWillBeNoFile()
		{
			Assert.False(File.Exists($"{filename}.csv"));
		}
	}
}
