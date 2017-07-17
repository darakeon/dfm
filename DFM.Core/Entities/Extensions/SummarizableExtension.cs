using System;
using System.Linq;
using DFM.Core.Database;
using DFM.Core.Entities.Bases;

namespace DFM.Core.Entities.Extensions
{
    public static class SummarizableExtension
    {
        public static Double Sum(this ISummarizable summarizable)
        {
            return summarizable.SummaryList.Sum(s => s.Value());
        }


        internal static Summary AjustSummaryList(this ISummarizable summarizable, Category category)
        {
            return summarizable.getSummary(category)
                ?? summarizable.AddSummary(category);
        }



        internal static Summary GetOrCreateSummary(this ISummarizable summarizable, Category category)
        {
            return summarizable.getSummary(category)
                   ?? summarizable.AddSummary(category);
        }

        private static Summary getSummary(this ISummarizable summarizable, Category category)
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
                    SummaryData.Delete(summary);
                }

                return null;
            }
        }

    }
}
