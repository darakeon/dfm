using System;
using DFM.Entities;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class AcceptanceRepository : BaseRepositoryLong<Acceptance>
	{
		internal void Accept(User user, Contract contract)
		{
			var acceptance = GetOrCreate(user, contract);

			if (acceptance == null)
				return;

			acceptance.Accepted = true;
			acceptance.AcceptDate = DateTime.Now;

			SaveOrUpdate(acceptance);
		}

		internal Acceptance GetOrCreate(User user, Contract contract)
		{
			if (user == null || contract == null)
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
