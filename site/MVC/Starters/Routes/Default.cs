using System;
using JetBrains.Annotations;

namespace DFM.MVC.Starters.Routes
{
	public abstract class Default : BaseRoute
	{
		protected Default() { }

		protected Default(
			[AspMvcController] String controller,
			[AspMvcAction] String action
		) : base(controller, action) { }

		public override String Area => Route.DefaultArea;

		public class Main : Default
		{
			public override String Path =>
				"{controller=Users}/{action=Index}/{id?}";
		}

		public class Mobile : Default
		{
			public Mobile() : base("Generic", "Mobile") { }
			public override String Path => "@{activity}";
		}
	}
}
