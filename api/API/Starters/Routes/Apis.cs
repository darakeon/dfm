using System;

namespace DFM.API.Starters.Routes
{
	public abstract class Apis : BaseRoute
	{
		public override String Area => "Default";

		public class Main : Apis
		{
			public override String Path =>
				"{controller=Status}/{action=Index}/{id?}";
		}

		public class Accounts : Apis
		{
			public override String Path =>
				"account-{accountUrl}/{controller=Moves}/{action=List}/{id?}";
		}
	}
}
