using System;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class MonthService : BaseService<Month>
    {
        protected internal MonthService(IRepository<Month> repository) : base(repository) { }

        internal Month GetOrCreateMonth(Int16 dateMonth, Year year, Category category = null)
        {
            var newMonth = getOrCreateMonth(year, dateMonth);

            if (category != null)
                newMonth.AjustSummaryList(category);

            return newMonth;
        }

        private Month getOrCreateMonth(Year year, Int16 dateMonth)
        {
            var monthList = year.MonthList
                .Where(m => m.Time == dateMonth);

            return monthList.SingleOrDefault()
                ?? createMonth(year, dateMonth);
        }

        private Month createMonth(Year year, Int16 month)
        {
            var newMonth = new Month { Year = year, Time = month };

            year.MonthList.Add(newMonth);

            SaveOrUpdate(newMonth);

            return newMonth;
        }


        internal void SaveOrUpdate(Month month)
        {
            base.SaveOrUpdate(month);
        }

        internal void RemoveMoveFromMonth(Move move)
        {
            if (move == null) return;

            if (move.In != null)
            {
                move.In.InList.Remove(move);
                SaveOrUpdate(move.In);
            }

            if (move.Out != null)
            {
                move.Out.OutList.Remove(move);
                SaveOrUpdate(move.Out);
            }
        }


    }
}
