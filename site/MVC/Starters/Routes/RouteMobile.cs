using System;

namespace DFM.MVC.Starters.Routes
{
	public class RouteMobile : BaseRoute
	{
		public override String Area => Route.DefaultArea;
		public override String Path => "@{activity}";
		public override object Defaults =>
			new { controller = "Users", action = "Mobile" };
	}
}
