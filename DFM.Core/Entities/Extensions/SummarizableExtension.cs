using System;
using System.Linq;
using DFM.Core.Entities.Base;

namespace DFM.Core.Entities.Extensions
{
    public static class SummarizableExtension
    {
        internal static Double Value(this ISummarizable summarizable)
        {
            return summarizable.SummaryList.Sum(s => s.Value);
        }


        internal static Summary AjustSummaryList(this ISummarizable summarizable, Category category)
        {
            return summarizable.SummaryList
                    .SingleOrDefault(s => s.Category == category)
                ?? summarizable.AddSummary(category);
        }

    }
}
