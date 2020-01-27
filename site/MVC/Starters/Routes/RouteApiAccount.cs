using System;

namespace DFM.MVC.Starters.Routes
{
	public class RouteApiAccount : BaseRoute
	{
		public override String Area => Route.ApiArea;
		public override String Path =>
			"api/account-{accountUrl}/{controller=Moves}/{action=List}/{id?}";
	}
}
