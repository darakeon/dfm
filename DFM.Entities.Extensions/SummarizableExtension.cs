using System;
using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities.Extensions
{
    public static class SummarizableExtension
    {
        public static Double Sum(this ISummarizable summarizable)
        {
            return summarizable.SummaryList.Sum(s => s.Value());
        }



        public static Summary AjustSummaryList(this ISummarizable summarizable, Category category)
        {
            return summarizable.getSummary(category)
                ?? summarizable.AddSummary(category);
        }



        public static Summary GetOrCreateSummary(this ISummarizable summarizable, Category category)
        {
            return summarizable.getSummary(category)
                   ?? summarizable.AddSummary(category);
        }

        private static Summary getSummary(this ISummarizable summarizable, Category category)
        {
            return summarizable.SummaryList
                .SingleOrDefault(s => s.Category == category);
        }

    }
}
