using Ak.MVC.Route;
using DFM.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Android.Models
{
    internal class BaseJsonModel : BaseModel
    {
        private User user;

        protected User User
        {
            get
            {
                if (user == null)
                {
                    var routeInfo = new RouteInfo();

                    var ticket = routeInfo.RouteData.Values["ticket"].ToString();

                    user = Safe.GetUserByTicket(ticket);
                }

                return user;
            }
        }


    }
}
