using Keon.NHibernate.Operations;

namespace DFM.BusinessLogic.Services
{
	public class Service : NHService
	{
		protected Service(ServiceAccess serviceAccess)
		{
			parent = serviceAccess;
		}

		protected ServiceAccess parent { get; }
	}
}
