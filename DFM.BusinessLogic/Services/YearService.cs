using System;
using System.Linq;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Services
{
    internal class YearService : BaseService<Year>
    {
        internal YearService(IRepository<Year> repository) : base(repository) { }

        internal Year GetOrCreateYear(Int16 year, Account account, Category category = null)
        {
            var newYear = getOrCreateYear(account, year);

            if (category != null)
                newYear.GetOrCreateSummary(category);

            return newYear;
        }

        private Year getOrCreateYear(Account account, Int16 dateYear)
        {
            var yearList = account.YearList
                .Where(m => m.Time == dateYear);

                return yearList.SingleOrDefault()
                    ?? createYear(account, dateYear);
        }

        private Year createYear(Account account, Int16 year)
        {
            var newYear = new Year{ Account = account, Time = year };

            account.YearList.Add(newYear);
            SaveOrUpdate(newYear);

            return newYear;
        }


        internal void SaveOrUpdate(Year year)
        {
            base.SaveOrUpdate(year);
        }


    }
}
