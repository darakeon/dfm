using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Database
{
    internal class MonthData : BaseData<Month>
    {
		private MonthData() { }

        internal static Month GetOrCreateMonth(Int16 dateMonth, Year year, Category category = null)
        {
            var newMonth = getMonth(year, dateMonth);

            if (category != null)
                newMonth.AjustSummaryList(category);

            return newMonth;
        }

        private static Month getMonth(Year year, Int16 dateMonth)
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

        private static Month createMonth(Year year, Int16 month)
        {
            var newMonth = new Month { Year = year, Time = month };

            year.MonthList.Add(newMonth);
            SaveOrUpdate(newMonth);

            return newMonth;
        }


        public static Month SaveOrUpdate(Month month)
        {
            return SaveOrUpdate(month, null, null);
        }

    }
}
