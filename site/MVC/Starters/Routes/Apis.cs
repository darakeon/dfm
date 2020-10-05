using System;

namespace DFM.MVC.Starters.Routes
{
	public abstract class Apis : BaseRoute
	{
		public override String Area => Route.ApiArea;

		public class Main : Apis
		{
			public override String Path =>
				"api/{controller=Status}/{action=Index}/{id?}";
		}

		public class Accounts : Apis
		{
			public override String Path =>
				"api/account-{accountUrl}/{controller=Moves}/{action=List}/{id?}";
		}
	}
}
