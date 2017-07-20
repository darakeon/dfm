using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Extensions.Entities;

namespace DFM.BusinessLogic.Services
{
    public class YearService : BaseService<Year>
    {
        internal YearService(DataAccess father, IRepository repository) : base(father, repository) { }

        public Year GetOrCreateYear(Int16 year, Account account, Category category = null)
        {
            var newYear = getOrCreateYear(account, year);

            if (category != null)
                newYear.AjustSummaryList(category, Father.Summary.Delete);

            return newYear;
        }

        private Year getOrCreateYear(Account account, Int16 dateYear)
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

        private Year createYear(Account account, Int16 year)
        {
            var newYear = new Year{ Account = account, Time = year };

            account.YearList.Add(newYear);
            SaveOrUpdate(newYear);

            return newYear;
        }


        public void SaveOrUpdate(Year year)
        {
            SaveOrUpdateInstantly(year, null, null);
        }


    }
}
