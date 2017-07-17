using System;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Database
{
    internal class MonthData : BaseData<Month>
    {
		private MonthData() { }

        internal static Month GetOrCreateMonth(Int16 month, Year year, Category category = null)
        {
            var newMonth = year.GetMonth(month)
                ?? createMonth(year, month);

            if (category != null)
                newMonth.AjustSummaryList(category);

            return newMonth;
        }

        private static Month createMonth(Year year, Int16 month)
        {
            var newMonth = new Month { Year = year, Time = month };

            year.MonthList.Add(newMonth);

            return newMonth;
        }


        public static Month SaveOrUpdate(Month month)
        {
            return SaveOrUpdate(month, null, null);
        }

    }
}
