using System;
using System.Linq;
using DFM.Core.Entities.Base;
using DFM.Core.Helpers;

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
            return summarizable.GetSummary(category)
                ?? summarizable.AddSummary(category);
        }



        internal static Summary GetSummary(this ISummarizable summarizable, Category category)
        {
            try
            {
                return summarizable.SummaryList
                    .SingleOrDefault(s => s.Category == category);
            }
            catch (InvalidOperationException e)
            {
                var nature = summarizable.SummaryList.First().Nature;
                var error = String.Format("SummaryAmbiguousIn{0}", nature);

                if (e.Message == "Sequence contains more than one matching element")
                    throw new DFMCoreException(error);
                throw;
            }
        }

    }
}
