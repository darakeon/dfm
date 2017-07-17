using System;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class MonthData : BaseData<Month>
    {
        internal Month GetOrCreateMonth(Int32 month, Year year, Category category)
        {
            var newMonth = year.MonthList
                    .SingleOrDefault(y => y.Time == month)
                ?? createMonth(year, month);

            if (category != null)
                newMonth.AjustSummaryList(category);

            return newMonth;
        }

        private Month createMonth(Year year, Int32 month)
        {
            var newMonth = new Month { Year = year, Time = month };

            year.MonthList.Add(newMonth);

            SaveOrUpdate(newMonth);

            return newMonth;
        }
    }
}
