using DFM.BusinessLogic.Repositories;
using Keon.NHibernate.Operations;

namespace DFM.BusinessLogic.Services
{
	public class Service : NHService
	{
		private protected Service(ServiceAccess parent, Repos repos)
		{
			this.parent = parent;
			this.repos = repos;
		}

		protected ServiceAccess parent { get; }
		private protected Repos repos { get; }
	}
}
