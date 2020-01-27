using System;

namespace DFM.MVC.Starters.Routes
{
	public class RouteApi : BaseRoute
	{
		public override String Area => Route.ApiArea;
		public override String Path =>
			"api/{controller=Status}/{action=Index}/{id?}";
	}
}
