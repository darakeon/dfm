using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Validators;
using Keon.NHibernate.Operations;

namespace DFM.BusinessLogic.Services
{
	public class Service : NHService
	{
		private protected Service(ServiceAccess parent, Repos repos, Valids valids)
		{
			this.parent = parent;
			this.repos = repos;
			this.valids = valids;
		}

		protected ServiceAccess parent { get; }
		private protected Repos repos { get; }
		private protected Valids valids { get; }
	}
}
