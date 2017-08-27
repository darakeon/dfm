using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.Repositories;

namespace DFM.MVC.Helpers.Authorize
{
    public class Auth
    {
        private static readonly ServiceAccess access = new ServiceAccess(new Connector());

        public static readonly Current Current = new Current(access.Safe);

    }

}