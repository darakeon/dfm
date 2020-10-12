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

		public virtual Move GetNewMove()
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
				};

			var dateTime = LastDateRun();
			move.SetDate(dateTime);

			if (!DetailList.Any())
				move.Value = Value;

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
			var date = this.GetDate();

			switch (Frequency)
			{
				case ScheduleFrequency.Daily:
					return date.AddDays(LastRun);
				case ScheduleFrequency.Monthly:
					return date.AddMonths(LastRun);
				case ScheduleFrequency.Yearly:
					return date.AddYears(LastRun);
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
