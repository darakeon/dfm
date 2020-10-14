using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
	public partial class Schedule
	{
		private void init()
		{
			Times = 1;

			MoveList = new List<Move>();
			DetailList = new List<Detail>();

			Active = true;
			Frequency = ScheduleFrequency.Monthly;

			ExternalId = new Byte[16];
		}

		public override string ToString()
		{
			return $"[{ID}] {Frequency} x {Description}";
		}

		public virtual Guid Guid
		{
			get => new Guid(ExternalId);
			set => ExternalId = value.ToByteArray();
		}

		public virtual Decimal Value
		{
			get => ValueCents.ToVisual();
			set => ValueCents = value.ToCents();
		}

		public virtual Move CreateMove()
		{
			var move = createMove(
				DetailList.Any() ? 0 : ValueCents
			);

			move.SetDate(LastDateRun());

			return move;
		}

		public virtual IEnumerable<Move> CreateMovesByFrequency(Int16 dateYear, Int16 dateMonth)
		{
			var firstMonthDay = new DateTime(dateYear, dateMonth, 1);
			var firstNextMonthDay = firstMonthDay.AddMonths(1);
			var lastMonthDay = firstNextMonthDay.AddDays(-1);

			var begin = LastDateRun();
			if (begin > lastMonthDay)
				yield break;

			var end = Boundless ? firstNextMonthDay : add(Times-1);
			if (end < firstMonthDay)
				yield break;

			switch (Frequency)
			{
				case ScheduleFrequency.Yearly:
				{
					if (Month == dateMonth)
						yield return createMove(dateYear, dateMonth);

					break;
				}

				case ScheduleFrequency.Monthly:
				{
					yield return createMove(dateYear, dateMonth);
					break;
				}

				case ScheduleFrequency.Daily:
				{
					if (begin < firstMonthDay)
						begin = firstMonthDay;

					if (end > lastMonthDay)
						end = lastMonthDay;

					for (var day = (Int16)begin.Day; day <= end.Day; day++)
					{
						yield return createMove(dateYear, dateMonth, day);
					}

					break;
				}

				default:
					throw new ArgumentException("frequency");
			}
		}

		public virtual Int32 PreviewSumUntil(Account account, Int16 dateYear, Int16 dateMonth)
		{
			var firstMonthDay = new DateTime(dateYear, dateMonth, 1);
			var firstNextMonthDay = firstMonthDay.AddMonths(1);
			var lastMonthDay = firstNextMonthDay.AddDays(-1);

			var value = 0;

			var begin = LastDateRun();
			var times = Boundless ? Int32.MaxValue : Times;
			var run = times - LastRun;

			var date = begin;
			while (date < lastMonthDay && run > 0)
			{
				value += ValueCents;
				date = add(date)(1);
				run--;
			}

			if (account == Out)
			{
				value *= -1;
			}

			return value;
		}

		public virtual Int32 PreviewSumAt(Account account, Int16 dateYear, Int16 dateMonth)
		{
			var firstMonthDay = new DateTime(dateYear, dateMonth, 1);
			var firstNextMonthDay = firstMonthDay.AddMonths(1);
			var lastMonthDay = firstNextMonthDay.AddDays(-1);

			var begin = LastDateRun();
			if (begin > lastMonthDay)
				return 0;

			var end = Boundless ? firstNextMonthDay : add(Times - 1);
			if (end < firstMonthDay)
				return 0;

			switch (Frequency)
			{
				case ScheduleFrequency.Yearly:
					return Month == dateMonth ? ValueCents : 0;

				case ScheduleFrequency.Monthly:
					return ValueCents;

				case ScheduleFrequency.Daily:
				{
					if (begin < firstMonthDay)
						begin = firstMonthDay;

					if (end > lastMonthDay)
						end = lastMonthDay;

					return ValueCents * (end.Day - begin.Day + 1);
				}

				default:
					throw new ArgumentException("frequency");
			}
		}

		private Move createMove(Int16 year, Int16 month, Int16? day = null)
		{
			var move = createMove(ValueCents);
			move.Year = year;
			move.Month = month;
			move.Day = day ?? Day;

			return move;
		}

		private Move createMove(Int32 value)
		{
			var move =
				new Move
				{
					Description = Description,
					Nature = Nature,
					Schedule = this,
					In = In,
					Out = Out,
					Category = Category,
					ValueCents = value,
				};

			foreach (var detail in DetailList)
			{
				var newDetail = detail.Clone();
				newDetail.Move = move;
				move.DetailList.Add(newDetail);
			}

			return move;
		}

		public virtual Boolean CanRun()
		{
			return canRun(false);
		}

		public virtual Boolean CanRunNow()
		{
			return canRun(true);
		}

		private Boolean canRun(Boolean tryNow)
		{
			return Active
			    && (Boundless || LastRun < Times)
			    && (!tryNow || LastDateRun() < User.Now());
		}

		public virtual DateTime LastDateRun()
		{
			return add(LastRun);
		}

		private DateTime add(Int32 count)
		{
			return add(this.GetDate())(count);
		}

		private Func<Int32, DateTime> add(DateTime date)
		{
			switch (Frequency)
			{
				case ScheduleFrequency.Daily:
					return d => date.AddDays(d);
				case ScheduleFrequency.Monthly:
					return date.AddMonths;
				case ScheduleFrequency.Yearly:
					return date.AddYears;
				default:
					throw new ArgumentException("schedule");
			}
		}
	}
}
