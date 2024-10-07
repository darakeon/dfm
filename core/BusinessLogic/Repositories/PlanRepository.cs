using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class PlanRepository : Repo<Plan>
	{
		public Plan GetFree()
		{
			return NewQuery()
				.Where(p => p.PriceCents == 0)
				.FirstOrDefault;
		}
	}
}
