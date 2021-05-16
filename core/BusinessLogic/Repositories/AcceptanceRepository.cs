using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class AcceptanceRepository : Repo<Acceptance>
	{
		internal Boolean Accept(User user, Contract contract)
		{
			var acceptance = GetOrCreate(user, contract);

			if (acceptance == null || acceptance.Accepted)
				return false;

			acceptance.Accepted = true;
			acceptance.AcceptDate = DateTime.UtcNow;

			SaveOrUpdate(acceptance);

			return true;
		}

		internal Acceptance GetOrCreate(User user, Contract contract)
		{
			if (user == null || contract == null)
				return null;

			return get(user, contract)
				?? create(user, contract);
		}

		private Acceptance get(User user, Contract contract)
		{
			return NewQuery()
				.Where(
					a => a.User.ID == user.ID
						&& a.Contract.ID == contract.ID
				)
				.SingleOrDefault;
		}

		private Acceptance create(User user, Contract contract)
		{
			var acceptance = new Acceptance
			{
				Contract = contract,
				User = user,
				CreateDate = user.Now()
			};

			SaveOrUpdate(acceptance);

			return acceptance;
		}
	}
}
