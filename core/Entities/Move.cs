using System;
using System.Collections.Generic;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
	public class Move : IMove
	{
		public Move()
		{
			DetailList = new List<Detail>();
			ExternalId = new Byte[16];
		}

		public virtual Int64 ID { get; set; }
		public virtual Byte[] ExternalId { get; set; }

		public virtual String Description { get; set; }
		public virtual MoveNature Nature { get; set; }
		public virtual Int32 ValueCents { get; set; }
		public virtual Int32? ConversionCents { get; set; }

		public virtual Int16 Day { get; set; }
		public virtual Int16 Month { get; set; }
		public virtual Int16 Year { get; set; }

		public virtual Int16? Position { get; set; }

		public virtual Boolean CheckedIn { get; set; }
		public virtual Boolean CheckedOut { get; set; }

		public virtual Category Category { get; set; }
		public virtual Schedule Schedule { get; set; }

		public virtual IList<Detail> DetailList { get; set; }

		public virtual Account In { get; set; }
		public virtual Account Out { get; set; }

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

		public virtual String GetDescriptionWithSchedulePosition()
		{
			if (Schedule is not {ShowInstallment: true})
				return Description;

			const String boundlessFormat = "{0} [{1}]";
			const String boundedFormat = "{0} [{1}/{2}]";

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

		public override Boolean Equals(Object obj)
		{
			return obj is Move move
				&& move.ID == ID;
		}

		public override Int32 GetHashCode()
		{
			// ReSharper disable once NonReadonlyMemberInGetHashCode
			return ID.GetHashCode();
		}

		public override String ToString()
		{
			return $"[{ID}] {Description}";
		}
	}
}
