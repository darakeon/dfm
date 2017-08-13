using System;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Extensions;

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

            if (!summaryList.Any())
                return;

            removeRepeated(summarizable);

            foreach (var summary in summaryList)
            {
                fix(summary, summarizable);
            }
        }

        private void fix(Summary summary, ISummarizable summarizable)
        {
            summary.In = summarizable.CheckUpIn(summary.Category);
            summary.Out = summarizable.CheckUpOut(summary.Category);
            summary.Broken = false;

            SaveOrUpdate(summary);
        }

        private void removeRepeated(ISummarizable summarizable)
        {
            var grouped = summarizable.SummaryList
                .GroupBy(s => s.UniqueID());

            foreach (var group in grouped)
            {
                if (group.Count() > 1)
                {
                    var firstSummary = group.First();

                    firstSummary.Broken = true;
                    
                    var otherSummaries = group.Skip(1);

                    foreach (var summary in otherSummaries)
                    {
                        summarizable.SummaryList.Remove(summary);

                        Delete(summary);
                    }
                }
            }

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
