using DFM.BusinessLogic;

namespace DFM.Repositories
{
    public class Service
    {
        private static DataAccess access;

        public static DataAccess Access
        {
            get
            {
                return access 
                    ?? (access = new DataAccess( new Connector() ));
            }
        }


    }
}