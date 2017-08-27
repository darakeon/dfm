using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Extensions;
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

		public virtual Double? Value
		{
			get { return ValueCents.ToVisual(); }
			set { ValueCents = value.ToCents(); }
		}

		public virtual Double Total()
		{
			return Value ??
				DetailList.Sum(d => d.Value * d.Amount);
		}


		public virtual Int64 FakeID
		{
			get
			{
				return ID * Constants.FakeID;
			}
			set
			{
				if (value % Constants.FakeID != 0)
					throw new DFMException("Get back!");

				ID = (Int32)(value / Constants.FakeID);
			}
		}



		public virtual User User
		{
			get
			{
				var month = (Out ?? In);

				return month == null
					? null
					: month.Year.Account.User;
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
			return month == null ? null : month.Year.Account;
		}




		public virtual String Month()
        {
            return Date.ToString("MMMM");
        }




        public virtual void MakePseudoDetail(Double value)
        {
            var id = (DetailList.FirstOrDefault() ?? new Detail()).ID;

            DetailList = new List<Detail>();

            var detail = new Detail { ID = id, Description = Description, Value = value };

            AddDetail(detail);
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
            var executed = this.PositionInSchedule();
                
            var format = schedule.Boundless
                             ? boundlessFormat
                             : boundedFormat;

            return String.Format(format, Description, executed, total);
        }


    }
}
