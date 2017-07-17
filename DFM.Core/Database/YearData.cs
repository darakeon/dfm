using System;
using System.Linq;
using DFM.Core.Database.Bases;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class YearData : BaseData<Year>
    {
        internal Year GetOrCreateYear(Int32 year, Account account, Category category)
        {
            var newYear = account.YearList.SingleOrDefault(y => y.Time == year);

            if (newYear == null)
            {
                newYear = new Year{ Account = account, Time = year };

                account.YearList.Add(newYear);

                SaveOrUpdate(newYear);
            }

            if (category != null)
                newYear.AjustSummaryList(category);

            return newYear;
        }
    }
}
