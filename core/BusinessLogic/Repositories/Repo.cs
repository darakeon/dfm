using System;
using Keon.NHibernate.Operations;
using Keon.Util.DB;

namespace DFM.BusinessLogic.Repositories
{
	public class Repo<Entity> : NHRepo<Entity, Int64>
		where Entity : class, IEntity<Int64>, new()
	{
		protected DateTime firstDayThisMonth =>
			DateTime.Today
				.AddDays(-DateTime.Today.Day)
				.AddDays(1);

		protected DateTime lastDayThisMonth =>
			firstDayThisMonth
				.AddMonths(1)
				.AddSeconds(-1);
	}
}
