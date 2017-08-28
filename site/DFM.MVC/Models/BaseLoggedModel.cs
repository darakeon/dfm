using System;
using DK.MVC.Route;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class BaseLoggedModel : BaseModel
	{
		public BaseLoggedModel()
		{
			ActionName = RouteInfo.Current["action"] ?? String.Empty;
		}

		public String CurrentMonth => MultiLanguage.GetMonthName(Today.Month);

		public String CurrentYear => Today.ToString("yyyy");

		public String ActionName { get; set; }

		public String Language => Current.Language;
	}
}