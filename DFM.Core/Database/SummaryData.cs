using System;
using System.Linq;
using DFM.Core.Entities.Base;
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
            var summaryMonth = getSummaryMonth(month, year, category, account)
                ?? createSummaryMonth(month, year, category, account);

            invalidate(summaryMonth);
        }


        private static Summary getSummaryMonth(Int32 monthDate, Int32 yearDate, Category category, Account account)
        {
            var year = account.YearList.SingleOrDefault(y => y.Time == yearDate && y.Account == account) ?? new Year();
            var month = year.MonthList.SingleOrDefault(m => m.Time == monthDate) ?? new Month();

            return month.SummaryList.SingleOrDefault(s => s.Category == category && s.Nature == SummaryNature.Month);
        }

        private static Summary createSummaryMonth(Int32 month, Int32 year, Category category, Account account)
        {
            var newYear = account.YearList.SingleOrDefault(y => y.Time == year);
            var newMonth = newYear.MonthList.SingleOrDefault(m => m.Time == month);

            return new Summary { Category = category, Month = newMonth, Nature = SummaryNature.Month };
        }



        private static void invalidateYear(Int32 year, Category category, Account account)
        {
            var summaryYear = getSummaryYear(year, category, account)
                ?? createSummaryYear(year, category, account);

            invalidate(summaryYear);
        }


        private static Summary getSummaryYear(Int32 yearDate, Category category, Account account)
        {
            var year = account.YearList.SingleOrDefault(y => y.Time == yearDate && y.Account == account) ?? new Year();

            return year.SummaryList.SingleOrDefault(s => s.Category == category && s.Nature == SummaryNature.Year);
        }

        private static Summary createSummaryYear(Int32 year, Category category, Account account)
        {
            var newYear = account.YearList.SingleOrDefault(y => y.Time == year);

            return new Summary { Category = category, Year = newYear, Nature = SummaryNature.Year };
        }



        private static void invalidate(Summary summary)
        {
            summary.IsValid = false;
            SaveOrUpdate(summary);
        }



        internal static void AjustValue(Summary summary)
        {
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

            summary.IsValid = true;

            SaveOrUpdate(summary);
        }



        internal static void Complete(Schedule schedule)
        {
            var move = schedule.MoveList.Last();

            schedule.Begin = move.Date;
            
            schedule.Next = 
                schedule.Frequency == ScheduleFrequency.Monthly 
                    ? move.Date.AddMonths(1)
                    : move.Date.AddYears(1);

            var user = (move.In ?? move.Out)
                            .Year.Account.User;

            schedule.User = user;
        }

    }
}
