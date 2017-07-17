using System;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Entities.Extensions;
using DFM.Core.Enums;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Helpers;

namespace DFM.Core.Database
{
    internal class SummaryData : BaseData<Summary>
    {
		private SummaryData() { }

        public static Summary SaveOrUpdate(Summary summary)
        {
            return SaveOrUpdate(summary, null, null);
        }



        internal static void SetSummary(ISummarizable summarizable, Category category)
        {
            var summary = summarizable.AjustSummaryList(category);

            summary.Value = summarizable.CheckUp(category);

            SaveOrUpdate(summary);
        }



        internal static void Ajust(Int32 month, Int32 year, Category category, Account account)
        {
            ajustMonth(month, year, category, account);
            ajustYear(year, category, account);
        }



        private static void ajustMonth(Int32 month, Int32 year, Category category, Account account)
        {
            var summaryMonth = getSummaryMonth(month, year, category, account)
                ?? createSummaryMonth(month, year, category, account);

            AjustValue(summaryMonth);
        }

        private static Summary getSummaryMonth(Int32 monthDate, Int32 yearDate, Category category, Account account)
        {
            var year = account.YearList.SingleOrDefault(y => y.Time == yearDate && y.Account == account) ?? new Year();
            var month = year.MonthList.SingleOrDefault(m => m.Time == monthDate) ?? new Month();

            return month.SummaryList.SingleOrDefault(s => s.Category == category);
        }

        private static Summary createSummaryMonth(Int32 month, Int32 year, Category category, Account account)
        {
            var newYear = account.YearList.SingleOrDefault(y => y.Time == year);
            var newMonth = newYear.MonthList.SingleOrDefault(m => m.Time == month);

            return new Summary { Category = category, Month = newMonth, Nature = SummaryNature.Month };
        }



        private static void ajustYear(Int32 year, Category category, Account account)
        {
            var summaryYear = getSummaryYear(year, category, account)
                ?? createSummaryYear(year, category, account);

            AjustValue(summaryYear);
        }


        private static Summary getSummaryYear(Int32 yearDate, Category category, Account account)
        {
            var year = account.YearList.SingleOrDefault(y => y.Time == yearDate && y.Account == account) ?? new Year();

            return year.SummaryList.SingleOrDefault(s => s.Category == category);
        }

        private static Summary createSummaryYear(Int32 year, Category category, Account account)
        {
            var newYear = account.YearList.SingleOrDefault(y => y.Time == year);

            return new Summary { Category = category, Year = newYear, Nature = SummaryNature.Year };
        }



        internal static void AjustValue(Summary summary)
        {
            SaveOrUpdate(summary);

            switch (summary.Nature)
            {
                case SummaryNature.Month:
                    summary.Value = summary.Month.CheckUp(summary.Category);
                    break;
                case SummaryNature.Year:
                    summary.Value = summary.Year.CheckUp(summary.Category);
                    break;
                default:
                    throw new DFMCoreException("SummaryNatureNotFound");
            }
        }

    }
}
