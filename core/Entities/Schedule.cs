using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
	public class Schedule : IMove
	{
		public Schedule()
		{
			Times = 1;

			MoveList = new List<Move>();
			DetailList = new List<Detail>();

			Active = true;
			Frequency = ScheduleFrequency.Monthly;

			ExternalId = new Byte[16];
		}

		public virtual Int64 ID { get; set; }
		public virtual Byte[] ExternalId { get; set; }

		public virtual String Description { get; set; }
		public virtual MoveNature Nature { get; set; }
		public virtual Int32 ValueCents { get; set; }
		public virtual Int32? ConversionCents { get; set; }

		public virtual Boolean ShowInstallment { get; set; }

		public virtual Int16 Year { get; set; }
		public virtual Int16 Month { get; set; }
		public virtual Int16 Day { get; set; }

		public virtual Int16 LastRun { get; set; }
		public virtual Int16 Deleted { get; set; }
		public virtual Int16 Times { get; set; }

		public virtual ScheduleFrequency Frequency { get; set; }
		public virtual Boolean Boundless { get; set; }

		public virtual Boolean Active { get; set; }


		public virtual Category Category { get; set; }
		public virtual Account In { get; set; }
		public virtual Account Out { get; set; }
		public virtual User User { get; set; }

		public virtual IList<Detail> DetailList { get; set; }
		public virtual IList<Move> MoveList { get; set; }

		public virtual Guid Guid
		{
			get => new(ExternalId);
			set => ExternalId = value.ToByteArray();
		}

		public virtual Decimal Value
		{
			get => ValueCents.ToVisual();
			set => ValueCents = value.ToCents();
		}

		public virtual Decimal? Conversion
		{
			get => ConversionCents.ToVisual();
			set => ConversionCents = value.ToCents();
		}

		public virtual Move CreateMove()
		{
			var move = createMove(
				DetailList.Any() ? 0 : ValueCents,
				DetailList.Any() ? null : ConversionCents,
				m => m.SetDate(LastDateRun())
			);

			LastRun++;

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

			var end = Boundless ? firstNextMonthDay : add(Times - 1);
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

			var value = 0;

			var begin = LastDateRun();
			var times = Boundless ? Int32.MaxValue : Times;
			var run = times - LastRun;

			var date = begin;

			var valueCents = valueToShow(account);

			while (date < firstNextMonthDay && run > 0)
			{
				value += valueCents;
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

			var valueCents = valueToShow(account);

			switch (Frequency)
			{
				case ScheduleFrequency.Yearly:
					return Month == dateMonth ? valueCents : 0;

				case ScheduleFrequency.Monthly:
					return valueCents;

				case ScheduleFrequency.Daily:
					{
						if (begin < firstMonthDay)
							begin = firstMonthDay;

						if (end > lastMonthDay)
							end = lastMonthDay;

						return valueCents * (end.Day - begin.Day + 1);
					}

				default:
					throw new ArgumentException("frequency");
			}
		}

		private int valueToShow(Account account)
		{
			return In?.ID == account.ID
					&& ConversionCents.HasValue
				? ConversionCents.Value
				: ValueCents;
		}

		private Move createMove(Int16 year, Int16 month, Int16? day = null)
		{
			return createMove(
				ValueCents,
				ConversionCents,
				move => {
					move.Year = year;
					move.Month = month;
					move.Day = day ?? Day;
				}
			);
		}

		private Move createMove(Int32 value, Int32? conversion, Action<Move> setDate)
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
					ConversionCents = conversion,
				};

			foreach (var detail in DetailList)
			{
				var newDetail = detail.Clone();
				newDetail.Move = move;
				move.DetailList.Add(newDetail);
			}

			setDate(move);

			move.SetPositionInSchedule();

			return move;
		}

		public virtual Boolean CanRunNow()
		{
			return CanRun(User.Now());
		}

		public virtual Boolean CanRun(DateTime? limitDate = null)
		{
			return Active
				&& (Boundless || LastRun < Times)
				&& (!limitDate.HasValue || LastDateRun() < limitDate);
		}

		public virtual DateTime LastDateRun()
		{
			return add(LastRun);
		}

		public virtual DateTime LastDateShouldRun()
		{
			return add(Times);
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

		public override String ToString()
		{
			return $"[{ID}] {Frequency} x {Description}";
		}
	}
}
