using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
	public partial class Move
	{
		private void init()
		{
			DetailList = new List<Detail>();
		}

		public override String ToString()
		{
			return Description;
		}

		public virtual Decimal? Value
		{
			get => ValueCents.ToVisual();
			set => ValueCents = value.ToCents();
		}

		public virtual Decimal Total()
		{
			return Value ?? DetailList.Sum(d => d.GetTotal());
		}

		public virtual Int64 FakeID
		{
			get => ID * Constants.FakeID;
			set
			{
				if (value % Constants.FakeID != 0)
					throw new SystemError("Get back!");

				ID = (Int32)(value / Constants.FakeID);
			}
		}

		public virtual String GetDescriptionWithSchedulePosition()
		{
			if (Schedule == null || !Schedule.ShowInstallment)
				return Description;

			const string boundlessFormat = "{0} [{1}]";
			const string boundedFormat = "{0} [{1}/{2}]";

			var total = Schedule.Times;
			var executed = positionInSchedule();

			var format = Schedule.Boundless ? boundlessFormat : boundedFormat;

			return String.Format(format, Description, executed, total);
		}

		private Int32 positionInSchedule()
		{
			var schedule = Schedule;

			var diff = 0;

			if (schedule == null)
				return diff;

			var days = Date - schedule.Date;
			var month = Date.Month - schedule.Date.Month;
			var year = Date.Year - schedule.Date.Year;

			switch (schedule.Frequency)
			{
				case ScheduleFrequency.Daily:
					diff = (Int32)days.TotalDays;
					break;

				case ScheduleFrequency.Monthly:
					diff = month + year * 12;
					break;

				case ScheduleFrequency.Yearly:
					diff = year;
					break;

				default:
					throw new NotImplementedException();
			}

			return diff + 1;
		}
	}
}
