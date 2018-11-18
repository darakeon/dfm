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
			get { return ValueCents.ToVisual(); }
			set { ValueCents = value.ToCents(); }
		}

		public virtual Decimal Total()
		{
			return Value ?? DetailList.Sum(d => d.GetTotal());
		}


		public virtual Int64 FakeID
		{
			get
			{
				return ID * Constants.FAKE_ID;
			}
			set
			{
				if (value % Constants.FAKE_ID != 0)
					throw new DFMException("Get back!");

				ID = (Int32)(value / Constants.FAKE_ID);
			}
		}



		public virtual User User
		{
			get
			{
				var month = Out ?? In;

				return month?.Year.Account.User;
			}
		}

		public virtual Account AccOut()
		{
			return getAccount(Out);
		}

		public virtual Account AccIn()
		{
			return getAccount(In);
		}

		public virtual void AddDetail(Detail detail)
		{
			DetailList.Add(detail);

			detail.Move = this;
		}

		private static Account getAccount(Month month)
		{
			return month?.Year.Account;
		}




		public virtual String Month()
		{
			return Date.ToString("MMMM");
		}




		public virtual Boolean AuthorizeCRUD(User user)
		{
			return User == user;
		}




		public virtual String GetDescriptionDetailed()
		{
			const string boundlessFormat = "{0} [{1}]";
			const string boundedFormat = "{0} [{1}/{2}]";
			var schedule = Schedule;

			if (schedule == null || !schedule.ShowInstallment)
				return Description;


			var total = schedule.Times;
			var executed = positionInSchedule();

			var format = schedule.Boundless ? boundlessFormat : boundedFormat;

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
