using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class ContractRepository : Repo<Contract>
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
