using DFM.BusinessLogic;

namespace DFM.Core
{
    public class Service
    {
        private static DataAccess access;

        public static DataAccess Access
        {
            get
            {
                return access 
                    ?? (access = new DataAccess( new Resolver() ));
            }
        }


    }
}