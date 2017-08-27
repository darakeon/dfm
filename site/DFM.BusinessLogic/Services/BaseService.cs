namespace DFM.BusinessLogic.Services
{
    public class BaseService : Ak.NHibernate.BaseService
    {
        protected BaseService(ServiceAccess serviceAccess)
        {
            Parent = serviceAccess;
        }

        protected ServiceAccess Parent { get; private set; }

        

    

    }
}
