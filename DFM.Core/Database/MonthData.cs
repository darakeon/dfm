using System;
using System.Linq;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class MonthData : BaseData<Month>
    {
        internal Month GetMonth(Year year, Int32 month)
        {
            var newMonth = year.MonthList.SingleOrDefault(y => y.Time == month);

            if (newMonth == null)
            {
                newMonth = new Month { Year = year, Time = month };
                year.MonthList.Add(newMonth);

                SaveOrUpdate(newMonth);
            }

            return newMonth;
        }
    }
}
