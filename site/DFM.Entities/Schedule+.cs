using System;
using System.Collections.Generic;
using System.Linq;
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
		}

		public override string ToString()
		{
			return $"[{ID}] {Frequency} x {Description}";
		}

		public virtual Decimal? Value
		{
			get => ValueCents.ToVisual();
			set => ValueCents = value.ToCents();
		}

		public virtual Decimal Total()
		{
			return Value ??
				DetailList.Sum(d => d.Value * d.Amount);
		}

		public virtual Move GetNewMove()
		{
			var dateTime = LastDateRun();

			var move =
				new Move
				{
					Date = dateTime,
					Description = Description,
					Nature = Nature,
					Schedule = this,
					Value = Value,
					In = In,
					Out = Out,
					Category = Category,
				};

			foreach (var detail in DetailList)
			{
				var newDetail = detail.Clone();
				newDetail.Move = move;
				move.DetailList.Add(newDetail);
			}

			return move;
		}


		public virtual DateTime LastDateRun()
		{
			switch (Frequency)
			{
				case ScheduleFrequency.Monthly:
					return Date.AddMonths(LastRun);
				case ScheduleFrequency.Yearly:
					return Date.AddYears(LastRun);
				case ScheduleFrequency.Daily:
					return Date.AddDays(LastRun);
				default:
					throw new ArgumentException("schedule");
			}
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
			if (!Active)
				return false;

			var lastDate = LastDateRun();

			if (tryNow && lastDate >= User.Now())
				return false;

			if (Boundless)
				return true;

			return LastRun < Times;
		}
	}
}
