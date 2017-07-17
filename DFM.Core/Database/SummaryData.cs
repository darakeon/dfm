using System;
using System.Linq;
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

        private static void saveOrUpdate(Summary summary)
        {
            //TODO: I shouldn't do it too
            var summaryTransac = Session.BeginTransaction();
            
            SaveOrUpdate(summary, null, null);

            summaryTransac.Commit();
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
            var year = account.GetYear(yearDate) ?? new Year();
            var month = year.GetMonth(monthDate) ?? new Month();

            return month.GetSummary(category);
        }

        private static Summary createSummaryMonth(Int32 monthDate, Int32 yearDate, Category category, Account account)
        {
            var year = account.GetYear(yearDate);
            var month = year.GetMonth(monthDate);

            return new Summary { Category = category, Month = month, Nature = SummaryNature.Month };
        }



        private static void ajustYear(Int32 year, Category category, Account account)
        {
            var summaryYear = getSummaryYear(year, category, account)
                ?? createSummaryYear(year, category, account);

            AjustValue(summaryYear);
        }


        private static Summary getSummaryYear(Int32 yearDate, Category category, Account account)
        {
            var year = account.GetYear(yearDate) ?? new Year();

            return year.GetSummary(category);
        }

        private static Summary createSummaryYear(Int32 yearDate, Category category, Account account)
        {
            var year = account.GetYear(yearDate);

            return new Summary { Category = category, Year = year, Nature = SummaryNature.Year };
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

            saveOrUpdate(summary);
        }

    }
}
