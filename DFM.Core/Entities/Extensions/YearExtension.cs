using System;
using System.Linq;
using DFM.Core.Database;
using DFM.Core.Helpers;

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

        internal static Month GetMonth(this Year year, Int32 month)
        {
            try
            {
                return year.MonthList
                    .SingleOrDefault(m => m.Time == month);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains more than one matching element")
                    throw DFMCoreException.WithMessage(DFMCoreException.Possibilities.MonthAmbiguousInYear);
                throw;
            }
        }

    }
}
