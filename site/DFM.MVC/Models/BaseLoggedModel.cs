using System;
using DK.MVC.Route;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class BaseLoggedModel : BaseModel
	{
		public BaseLoggedModel()
		{
		}

		public String CurrentMonth => MultiLanguage.GetMonthName(Today.Month);

		public String CurrentYear => Today.ToString("yyyy");

		public String ActionName => RouteInfo.Current["action"] ?? String.Empty;
		public String ControllerName => RouteInfo.Current["controller"] ?? String.Empty;

		public String Language => Current.Language;
	}
}