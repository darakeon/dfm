using System;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class YearData : BaseData<Year>
    {
        internal Year GetOrCreateYear(Int32 year, Account account, Category category)
        {
            var newYear = account.YearList
                    .SingleOrDefault(y => y.Time == year)
                ?? createYear(account, year);

            if (category != null)
                newYear.AjustSummaryList(category);

            return newYear;
        }

        private Year createYear(Account account, Int32 year)
        {
            var newYear = new Year{ Account = account, Time = year };

            account.YearList.Add(newYear);

            SaveOrUpdate(newYear);
            return newYear;
        }
    }
}
