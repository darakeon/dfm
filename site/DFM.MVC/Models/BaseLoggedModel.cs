using System;
using Ak.MVC.Route;
using DFM.Entities.Extensions;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class BaseLoggedModel : BaseModel
    {
        public BaseLoggedModel()
        {
            ActionName = RouteInfo.Current.RouteData == null
                ? String.Empty
                : RouteInfo.Current.RouteData.Values["action"].ToString();
        }

        public String CurrentMonth
        {
            get { return MultiLanguage.GetMonthName(Today.Month); }
        }

        public String CurrentYear
        {
            get { return Today.ToString("yyyy"); }
        }

        public String ActionName { get; set; }

        public DateTime Today
        {
            get { return Current.User.Now().Date; }
        }
        


    }
}