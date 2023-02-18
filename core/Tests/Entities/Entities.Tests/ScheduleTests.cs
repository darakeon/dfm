using System;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using NUnit.Framework;

namespace DFM.Generic.Tests
{
	public class ScheduleTests
	{
		[Test]
		public void CreateMovesByFrequency_Day31()
		{
			var schedule = new Schedule();
			schedule.SetDate(new DateTime(2023, 1, 31));
			schedule.Boundless = true;

			var moves = schedule
				.CreateMovesByFrequency(2023, 2)
				.ToList();

			Assert.AreEqual(1, moves.Count);

			Assert.AreEqual(
				new DateTime(2023, 2, 28),
				moves[0].GetDate()
			);
		}

		[Test]
		public void CreateMovesByFrequency_Month12()
		{
			var schedule = new Schedule();
			schedule.SetDate(new DateTime(2023, 1, 31));
			schedule.Boundless = true;

			var moves = schedule
				.CreateMovesByFrequency(2023, 12)
				.ToList();

			Assert.AreEqual(1, moves.Count);

			Assert.AreEqual(
				new DateTime(2023, 12, 31),
				moves[0].GetDate()
			);
		}
	}
}
