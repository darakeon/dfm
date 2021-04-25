using System;
using System.Collections.Generic;
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
			ExternalId = new Byte[16];
		}

		public override String ToString()
		{
			return Description;
		}

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

		public virtual String GetDescriptionWithSchedulePosition()
		{
			if (Schedule == null || !Schedule.ShowInstallment)
				return Description;

			const string boundlessFormat = "{0} [{1}]";
			const string boundedFormat = "{0} [{1}/{2}]";

			var format = Schedule.Boundless ? boundlessFormat : boundedFormat;

			return String.Format(format, Description, Position, Schedule.Times);
		}

		public virtual void SetPositionInSchedule()
		{
			if (Schedule == null)
				return;

			var days = this.GetDate() - Schedule.GetDate();
			var month = Month - Schedule.Month;
			var year = Year - Schedule.Year;

			Int32 position;

			switch (Schedule.Frequency)
			{
				case ScheduleFrequency.Daily:
					position = (Int32)days.TotalDays;
					break;

				case ScheduleFrequency.Monthly:
					position = month + year * 12;
					break;

				case ScheduleFrequency.Yearly:
					position = year;
					break;

				default:
					throw new NotImplementedException();
			}

			Position = (Int16)(position + 1);
		}

		public virtual void Check(PrimalMoveNature nature, Boolean check)
		{
			switch (nature)
			{
				case PrimalMoveNature.In:
					CheckedIn = check;
					break;
				case PrimalMoveNature.Out:
					CheckedOut = check;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public virtual Boolean IsChecked(PrimalMoveNature nature)
		{
			switch (nature)
			{
				case PrimalMoveNature.In:
					return CheckedIn;
				case PrimalMoveNature.Out:
					return CheckedOut;
				default:
					throw new NotImplementedException();
			}
		}

		public override Boolean Equals(object obj)
		{
			return obj is Move move
			    && move.ID == ID;
		}

		public override int GetHashCode()
		{
			// ReSharper disable once NonReadonlyMemberInGetHashCode
			return ID.GetHashCode();
		}
	}
}
