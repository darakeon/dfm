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

        public static Summary GetOrCreateSummary(this ISummarizable summarizable, Category category)
        {
            return summarizable[category.Name]
                ?? summarizable.AddSummary(category);
        }

    }
}
