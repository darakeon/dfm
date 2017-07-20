using System;
using DFM.Entities.Bases;
using DFM.Extensions.Entities;
using DFM.Core.Enums;
using DFM.Core.Database.Base;
using DFM.Entities;
using DFM.Core.Exceptions;

namespace DFM.Core.Database
{
    internal class SummaryData : BaseData<Summary>
    {
		private SummaryData() { }

        private static void saveOrUpdate(Summary summary)
        {
            SaveOrUpdateInstantly(summary, null, null);
        }



        internal static void Ajust(Int16 month, Int16 year, Category category, Account account)
        {
            ajustMonth(month, year, category, account);
            ajustYear(year, category, account);
        }



        private static void ajustMonth(Int16 monthDate, Int16 yearDate, Category category, Account account)
        {
            var year = YearData.GetOrCreateYear(yearDate, account);
            var month = MonthData.GetOrCreateMonth(monthDate, year);

            var summaryMonth = month.GetOrCreateSummary(category, Delete);

            AjustValue(summaryMonth);
        }

        private static void ajustYear(Int16 yearDate, Category category, Account account)
        {
            var year = YearData.GetOrCreateYear(yearDate, account);

            var summaryYear = year.GetOrCreateSummary(category, Delete);

            AjustValue(summaryYear);
        }



        internal static void AjustValue(Summary summary)
        {
            ISummarizable summarizable;

            switch (summary.Nature)
            {
                case SummaryNature.Month:
                    summarizable = summary.Month;
                    break;
                case SummaryNature.Year:
                    summarizable = summary.Year;
                    break;
                default:
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.SummaryNatureNotFound);
            }

            summary.In = summarizable.CheckUpIn(summary.Category);
            summary.Out = summarizable.CheckUpOut(summary.Category);

            saveOrUpdate(summary);
        }

    }
}
