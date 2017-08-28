using System;
using DK.MVC.Route;
using DFM.MVC.Helpers.Global;

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

        public String Language
        {
            get { return Current.Language; }
        }


    }
}