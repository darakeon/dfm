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



        public delegate void DeleteSummary(Summary summary);


        public static Summary AjustSummaryList(this ISummarizable summarizable, Category category, DeleteSummary deleteSummary)
        {
            return summarizable.getSummary(category, deleteSummary)
                ?? summarizable.AddSummary(category);
        }



        public static Summary GetOrCreateSummary(this ISummarizable summarizable, Category category, DeleteSummary deleteSummary)
        {
            return summarizable.getSummary(category, deleteSummary)
                   ?? summarizable.AddSummary(category);
        }

        private static Summary getSummary(this ISummarizable summarizable, Category category, DeleteSummary deleteSummary)
        {
            var list = summarizable.SummaryList
                .Where(s => s.Category == category);

            try
            {
                return list.SingleOrDefault();
            }
            catch (InvalidOperationException e)
            {
                if (!e.Message.StartsWith("Sequence contains more than one"))
                    throw;
                
                foreach (var summary in list)
                {
                    deleteSummary(summary);
                }

                return null;
            }
        }

    }
}
