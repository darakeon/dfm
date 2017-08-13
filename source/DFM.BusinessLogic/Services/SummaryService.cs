using System;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    internal class SummaryService : BaseService<Summary>
    {
        internal SummaryService(IRepository<Summary> repository) : base(repository) { }



        internal void Break(Year year, Category category)
        {
            var summary = getByYearAndCategory(year, category);

            @break(summary);
        }

        private Summary getByYearAndCategory(Year year, Category category)
        {
            return SingleOrDefault(
                s => s.Year.ID == year.ID
                     && s.Category.ID == category.ID);
        }


        internal void Break(Month month, Category category)
        {
            var summary = getByMonthAndCategory(month, category);

            @break(summary);
        }

        private Summary getByMonthAndCategory(Month month, Category category)
        {
            return SingleOrDefault(
                s => s.Month.ID == month.ID
                     && s.Category.ID == category.ID);
        }


        private void @break(Summary summary)
        {
            summary.Broken = true;

            SaveOrUpdate(summary);
        }



        public void Fix(ISummarizable summarizable)
        {
            var summaryList = 
                summarizable
                    .SummaryList
                    .Where(s => s.Broken);

            foreach (var summary in summaryList)
            {
                fix(summary);
            }
        }

        private void fix(Summary summary)
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

            SaveOrUpdate(summary);
        }



        internal void CreateIfNotExists(Month month, Category category)
        {
            if (month == null)
                return;


            var summaryMonth = getByMonthAndCategory(month, category);

            if (summaryMonth == null)
            {
                summaryMonth = new Summary(month, category);

                SaveOrUpdate(summaryMonth);
            }


            var summaryYear = getByYearAndCategory(month.Year, category);

            if (summaryYear == null)
            {
                summaryYear = new Summary(month.Year, category);

                SaveOrUpdate(summaryYear);
            }
        }


    }
}
