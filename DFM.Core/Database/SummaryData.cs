using System;
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
            var summaryTransac = Session.BeginTransaction();
            SaveOrUpdate(summary, null, null);
            summaryTransac.Commit();
        }



        internal static void Ajust(Int32 month, Int32 year, Category category, Account account)
        {
            ajustMonth(month, year, category, account);
            ajustYear(year, category, account);
        }



        private static void ajustMonth(Int32 monthDate, Int32 yearDate, Category category, Account account)
        {
            var year = YearData.GetOrCreateYear(yearDate, account);
            var month = MonthData.GetOrCreateMonth(monthDate, year);

            var summaryMonth = month.GetSummary(category)
                ?? new Summary { Category = category, Month = month, Nature = SummaryNature.Month };

            AjustValue(summaryMonth);
        }

        private static void ajustYear(Int32 yearDate, Category category, Account account)
        {
            var year = YearData.GetOrCreateYear(yearDate, account);

            var summaryYear = year.GetSummary(category)
                ?? new Summary { Category = category, Year = year, Nature = SummaryNature.Year };

            AjustValue(summaryYear);
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
