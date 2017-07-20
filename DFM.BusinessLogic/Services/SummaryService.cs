using System;
using DFM.Entities.Bases;
using DFM.Extensions.Entities;
using DFM.Core.Enums;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    internal class SummaryService : BaseService<Summary>
    {
        internal SummaryService(DataAccess father, IRepository repository) : base(father, repository) { }

        private void saveOrUpdate(Summary summary)
        {
            SaveOrUpdateInstantly(summary);
        }



        internal void Ajust(Int16 month, Int16 year, Category category, Account account)
        {
            ajustMonth(month, year, category, account);
            ajustYear(year, category, account);
        }



        private void ajustMonth(Int16 monthDate, Int16 yearDate, Category category, Account account)
        {
            var year = Father.Year.GetOrCreateYear(yearDate, account);
            var month = Father.Month.GetOrCreateMonth(monthDate, year);

            var summaryMonth = month.GetOrCreateSummary(category, Delete);

            AjustValue(summaryMonth);
        }

        private void ajustYear(Int16 yearDate, Category category, Account account)
        {
            var year = Father.Year.GetOrCreateYear(yearDate, account);

            var summaryYear = year.GetOrCreateSummary(category, Delete);

            AjustValue(summaryYear);
        }



        internal void AjustValue(Summary summary)
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
