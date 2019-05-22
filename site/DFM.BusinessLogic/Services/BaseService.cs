namespace DFM.BusinessLogic.Services
{
	public class BaseService : Keon.NHibernate.Base.BaseService
	{
		protected BaseService(ServiceAccess serviceAccess)
		{
			Parent = serviceAccess;
		}

		protected ServiceAccess Parent { get; private set; }





	}
}
