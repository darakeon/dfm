using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class PlanRepository : Repo<Plan>
	{
		public Plan GetFree()
		{
			return SingleOrDefault(p => p.PriceCents == 0);
		}
	}
}
