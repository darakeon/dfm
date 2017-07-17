using System;
using System.Linq;
using DFM.Core.Entities;
using DFM.Core.Entities.Interfaces;

namespace DFM.Core.Database
{
    public class SummaryData : BaseData<Summary>
    {
        public void AjustMonth(Month month, Month oldMonth, Category category)
        {
            ajustMonthOrYear(month, oldMonth, category);
        }

        public void AjustYear(Month month, Month oldMonth, Category category)
        {
            ajustMonthOrYear(
                (month ?? new Month()).Year,
                (oldMonth ?? new Month()).Year,
                category);
        }
        
        
        private void ajustMonthOrYear(ISummarizable summarizable, ISummarizable oldSummarizable, Category category)
        {
            if (summarizable != null)
                setSummary(summarizable, category);

            if (oldSummarizable != null && summarizable != oldSummarizable)
                setSummary(oldSummarizable, category);
        }

        private void setSummary(ISummarizable summarizable, Category category)
        {
            var summary = summarizable.SummaryList
                .SingleOrDefault(s => s.Category == category);

            if (summary == null)
            {
                summary = new Summary { Category = category };
                summarizable.AddSummary(summary);
            }

            summary.Value = summarizable.CheckUp(category);

            SaveOrUpdate(summary);
        }
    }
}
