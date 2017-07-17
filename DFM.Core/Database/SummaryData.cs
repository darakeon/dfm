using System;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Enums;
using DFM.Core.Database.Base;
using DFM.Core.Entities;

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
            var summary = summarizable.SummaryList
                .SingleOrDefault(s => s.Category == category);

            if (summary == null)
            {
                summary = new Summary { Category = category };
                summarizable.AjustSummaryList(summary);
            }

            summary.Value = summarizable.CheckUp(category);

            SaveOrUpdate(summary);
        }



        internal static void Invalidate(Int32 month, Int32 year, Category category, Account account)
        {
            invalidateYear(year, category, account);
            invalidateMonth(month, year, category, account);
        }



        private static void invalidateMonth(Int32 month, Int32 year, Category category, Account account)
        {
            var summaryMonth = SelectSingle(
                    s => s.Nature == SummaryNature.Month
                         && s.Month.Time == month
                         && s.Category.ID == category.ID,
                    s => s.Month.Year.Account.ID == account.ID
                ) ?? createSummaryMonth(month, year, category, account);

            invalidate(summaryMonth);
        }

        private static Summary createSummaryMonth(Int32 month, Int32 year, Category category, Account account)
        {
            var newYear = YearData.SelectSingle(y => y.Account.ID == account.ID && y.Time == year);
            var newMonth = MonthData.SelectSingle(m => m.Year.ID == newYear.ID && m.Time == month);

            return new Summary { Category = category, Month = newMonth, Nature = SummaryNature.Month };
        }


        private static void invalidateYear(Int32 year, Category category, Account account)
        {
            var summaryYear = SelectSingle(
                    s => s.Nature == SummaryNature.Year
                         && s.Year.Time == year
                         && s.Category.ID == category.ID
                         && s.Year.Account.ID == account.ID
                ) ?? createSummaryYear(year, category, account);

            invalidate(summaryYear);
        }

        private static Summary createSummaryYear(Int32 year, Category category, Account account)
        {
            var newYear = YearData.SelectSingle(y => y.Account.ID == account.ID && y.Time == year);

            return new Summary { Category = category, Year = newYear, Nature = SummaryNature.Month };
        }



        private static void invalidate(Summary summary)
        {
            summary.IsValid = false;
            SaveOrUpdate(summary);
        }
    }
}
