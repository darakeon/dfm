using System;
using DFM.MVC.Helpers.Extensions;

namespace DFM.MVC.Helpers
{
    public class DateFromInt
    {
        internal static Int16 GetDateYear(Int32? id, DateTime today)
        {
            var currentYear = (Int16)today.Year;

            var dateYear = id.HasValue
                               ? (Int16)(id.Value / 100)
                               : currentYear;

            return dateYear.ForceBetween(1900, currentYear);
        }

        internal static Int16 GetDateMonth(Int32? id, DateTime today)
        {
            var currentMonth = (Int16)today.Month;

            var dateMonth = id.HasValue
                                ? (Int16)(id.Value % 100)
                                : currentMonth;

            return dateMonth.ForceBetween(1, 12);
        }

        internal static Int16 GetDateYear(Int16? id, DateTime today)
        {
            var currentYear = (Int16)today.Year;

            var year = id ?? currentYear;

            return year.ForceBetween(1900, currentYear);
        }


    }
}