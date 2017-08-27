using System;
using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities.Extensions
{
    public static class SummarizableExtension
    {
        public static Decimal Total(this ISummarizable summarizable)
        {
            return summarizable.SummaryList.Sum(s => s.Value());
        }

        public static Summary GetOrCreateSummary(this ISummarizable summarizable, Category category)
        {
            var categoryName = category == null ? null : category.Name;

            return summarizable[categoryName]
                ?? summarizable.AddSummary(category);
        }

    }
}
