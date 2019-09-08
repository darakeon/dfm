using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Services
{
	public class BaseService : BaseServiceLong
	{
		protected BaseService(ServiceAccess serviceAccess)
		{
			Parent = serviceAccess;
		}

		protected ServiceAccess Parent { get; private set; }
	}
}
