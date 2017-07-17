using System;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Database
{
    internal class YearData : BaseData<Year>
    {
		private YearData() { }

        internal static Year GetOrCreateYear(Int32 year, Account account, Category category)
        {
            var newYear = account.YearList
                    .SingleOrDefault(y => y.Time == year)
                ?? createYear(account, year);

            if (category != null)
                newYear.AjustSummaryList(category);

            return newYear;
        }

        private static Year createYear(Account account, Int32 year)
        {
            var newYear = new Year{ Account = account, Time = year };

            account.YearList.Add(newYear);

            //SaveOrUpdate(newYear);
            
            return newYear;
        }


        public static Year SaveOrUpdate(Year year)
        {
            return SaveOrUpdate(year, null, null);
        }


    }
}
