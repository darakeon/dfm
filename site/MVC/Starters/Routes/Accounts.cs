using System;
using DFM.BaseWeb.Starters.Routes;

namespace DFM.MVC.Starters.Routes
{
	public class Accounts : BaseRoute
	{
		public const String AreaName = "Account";
		public override String Area => AreaName;
		public override String Path =>
			"Account/{accountUrl}/{controller=Reports}/{action=Index}/{id?}";
	}
}
