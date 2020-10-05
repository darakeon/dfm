using System;

namespace DFM.MVC.Starters.Routes
{
	public class Accounts : BaseRoute
	{
		public override String Area => Route.AccountArea;
		public override String Path =>
			"Account/{accountUrl}/{controller=Reports}/{action=Index}/{id?}";
	}
}
