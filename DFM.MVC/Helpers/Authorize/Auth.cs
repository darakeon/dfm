using DFM.Authentication;
using DFM.Repositories;

namespace DFM.MVC.Helpers.Authorize
{
    public class Auth
    {
        public static readonly Current Current = new Current(Services.Safe);
    }

}