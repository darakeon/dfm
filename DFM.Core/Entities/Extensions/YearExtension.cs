using System;
using System.Linq;
using DFM.Core.Database;

namespace DFM.Core.Entities.Extensions
{
    public static class YearExtension
    {
        internal static Year Clone(this Year year)
        {
            return new Year
            {
                Account = year.Account,
                MonthList = year.MonthList,
                SummaryList = year.SummaryList,
                Time = year.Time
            };
        }

    }
}
