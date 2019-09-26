using DFM.Entities;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class ContractRepository : BaseRepositoryLong<Contract>
	{
		internal Contract GetContract()
		{
			return NewQuery()
				.OrderBy(a => a.ID, false)
				.OrderBy(a => a.BeginDate, false)
				.FirstOrDefault;
		}

	}
}
