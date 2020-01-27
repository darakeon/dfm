using System;

namespace DFM.MVC.Starters.Routes
{
	public class RouteDefault : BaseRoute
	{
		public override String Area => Route.DefaultArea;
		public override String Path =>
			"{controller=Users}/{action=Index}/{id?}";
	}
}
