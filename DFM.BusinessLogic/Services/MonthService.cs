using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Extensions.Entities;

namespace DFM.BusinessLogic.Services
{
    internal class MonthService : BaseService<Month>
    {
        protected internal MonthService(DataAccess father, IRepository repository) : base(father, repository) { }

        internal Month GetOrCreateMonth(Int16 dateMonth, Year year, Category category = null)
        {
            var newMonth = getOrCreateMonth(year, dateMonth);

            if (category != null)
                newMonth.AjustSummaryList(category, Father.Summary.Delete);

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


        public void SaveOrUpdate(Month month)
        {
            SaveOrUpdateInstantly(month, null, null);
        }

    }
}
