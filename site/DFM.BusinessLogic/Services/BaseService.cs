using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Services
{
	public class BaseService : BaseServiceLong
	{
		protected BaseService(ServiceAccess serviceAccess)
		{
			parent = serviceAccess;
		}

		protected ServiceAccess parent { get; }
	}
}
