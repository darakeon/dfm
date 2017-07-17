using System;
using System.Linq;

namespace DFM.Core.Entities.Extensions
{
    public static class YearExtension
    {
        //internal static Double Value(this Year year)
        //{
        //    return year.SummaryList.Sum(s => s.FixValue);
        //}


        //internal static void AjustSummaryList(this Year year, Category category)
        //{
        //    if (!year.SummaryList.Any(s => s.Category == category))
        //        year.AddSummary(category);
        //}

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
