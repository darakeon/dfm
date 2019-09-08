using System;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Extensions;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class YearRepository : BaseRepositoryLong<Year>
	{
		internal Year GetOrCreateYearWithSummary(Int16 dateYear, Account account, Category category)
		{
			var year = GetOrCreateYear(dateYear, account);

			year.GetOrCreateSummary(category);

			return year;
		}

		internal Year GetOrCreateYear(Int16 dateYear, Account account)
		{
			var year = account.YearList
				.SingleOrDefault(m => m.Time == dateYear);

			return year ?? createYear(account, dateYear);
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
