using System;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Database
{
    internal class MonthData : BaseData<Month>
    {
		private MonthData() { }

        internal static Month GetOrCreateMonth(Int32 month, Year year, Category category)
        {
            var newMonth = year.MonthList
                    .SingleOrDefault(y => y.Time == month)
                ?? createMonth(year, month);

            if (category != null)
                newMonth.AjustSummaryList(category);

            return newMonth;
        }

        private static Month createMonth(Year year, Int32 month)
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
