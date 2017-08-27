using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Route;
using DFM.Entities.Extensions;
using DFM.Entities;
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

        public String CurrentMonth { get { return MultiLanguage.GetMonthName(DateTime.Now.Month); } }
        public String CurrentYear { get { return DateTime.Now.ToString("yyyy"); } }

        public String ActionName { get; set; }




    }
}