using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Core.Database.Base;
using DFM.Core.Entities;
using DFM.Core.Entities.Extensions;

namespace DFM.Core.Database
{
    internal class YearData : BaseData<Year>
    {
		private YearData() { }

        internal static Year GetOrCreateYear(Int16 year, Account account, Category category = null)
        {
            var newYear = getOrCreateYear(account, year);

            if (category != null)
                newYear.AjustSummaryList(category);

            return newYear;
        }

        private static Year getOrCreateYear(Account account, Int16 dateYear)
        {
            var yearList = account.YearList
                .Where(m => m.Time == dateYear);

            try
            {
                return yearList.SingleOrDefault()
                    ?? createYear(account, dateYear);
            }
            catch (InvalidOperationException e)
            {
                if (!e.Message.StartsWith("Sequence contains more than one"))
                    throw;

                var monthList = new List<Month>();

                foreach (var year in yearList)
                {
                    monthList.AddRange(year.MonthList);

                    Delete(year);
                }

                var newYear = createYear(account, dateYear);
                newYear.MonthList = monthList;

                return newYear;
            }
        }

        private static Year createYear(Account account, Int16 year)
        {
            var newYear = new Year{ Account = account, Time = year };

            account.YearList.Add(newYear);
            SaveOrUpdate(newYear);

            return newYear;
        }


        public static void SaveOrUpdate(Year year)
        {
            SaveOrUpdateInstantly(year, null, null);
        }


    }
}
