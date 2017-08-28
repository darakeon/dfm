using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class AcceptanceRepository : BaseRepository<Acceptance>
	{
		internal void Accept(User user, Contract contract)
		{
			var acceptance = GetOrCreate(user, contract);

			acceptance.Accepted = true;

			SaveOrUpdate(acceptance);
		}

		internal Acceptance GetOrCreate(User user, Contract contract)
		{
			if (user == null)
				return null;

			var acceptance = NewQuery()
				.SimpleFilter(a => a.User.ID == user.ID && a.Contract.ID == contract.ID)
				.UniqueResult;

			if (acceptance == null)
			{
				acceptance = new Acceptance
				{
					Contract = contract,
					User = user,
					CreateDate = user.Now()
				};

				SaveOrUpdate(acceptance);
			}

			return acceptance;
		}

	}
}