using System;
using System.Linq;
using DFM.Core.Entities;

namespace DFM.Core.Database
{
    public class YearData : BaseData<Year>
    {
        internal Year GetYear(Account account, Int32 year)
        {
            var newYear = account.YearList.SingleOrDefault(y => y.Time == year);

            if (newYear == null)
            {
                newYear = new Year{ Account = account, Time = year };
                account.YearList.Add(newYear);

                SaveOrUpdate(newYear);
            }

            return newYear;
        }
    }
}
