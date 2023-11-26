using System;
using System.Linq;
using DFM.Entities.Bases;
using NUnit.Framework;

namespace DFM.Entities.Tests
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

			Assert.That(moves.Count, Is.EqualTo(1));

			Assert.That(
				moves[0].GetDate(),
				Is.EqualTo(new DateTime(2023, 2, 28))
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

			Assert.That(moves.Count, Is.EqualTo(1));

			Assert.That(
				moves[0].GetDate(),
				Is.EqualTo(new DateTime(2023, 12, 31))
			);
		}
	}
}
