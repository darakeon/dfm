using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class MonthService : BaseService<Month>
    {
        protected internal MonthService(IRepository<Month> repository) : base(repository) { }

        internal Month GetOrCreateMonth(Int16 dateMonth, Year year, SummarizableExtension.DeleteSummary deleteSummary, Category category = null)
        {
            var newMonth = getOrCreateMonth(year, dateMonth);

            if (category != null)
                newMonth.AjustSummaryList(category, deleteSummary);

            return newMonth;
        }

        private Month getOrCreateMonth(Year year, Int16 dateMonth)
        {
            var monthList = year.MonthList
                .Where(m => m.Time == dateMonth);

            try
            {
                return monthList.SingleOrDefault()
                    ?? createMonth(year, dateMonth);
            }
            catch (InvalidOperationException e)
            {
                if (!e.Message.StartsWith("Sequence contains more than one"))
                    throw;

                var outList = new List<Move>();
                var inList = new List<Move>();

                foreach (var month in monthList)
                {
                    outList.AddRange(month.OutList);
                    inList.AddRange(month.InList);

                    Delete(month);
                }

                var newMonth = createMonth(year, dateMonth);
                newMonth.InList = inList;
                newMonth.OutList = outList;

                return newMonth;
            }
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
            SaveOrUpdateInstantly(month);
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
