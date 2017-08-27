using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Extensions;

namespace DFM.Entities
{
    public partial class Move
    {
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
