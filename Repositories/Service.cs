using DFM.BusinessLogic;

namespace DFM.Repositories
{
    public class Service
    {
        private static ServiceAccess access;

        public static ServiceAccess Access
        {
            get
            {
                return access 
                    ?? (access = new ServiceAccess( new Connector() ));
            }
        }


    }
}