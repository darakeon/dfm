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

		public class Contract : Default
		{
			public Contract() : base("Users", "Contract") { }
			public override String Path => "contract";
		}

		public class Mail : Default
		{
			public Mail() : base("Tokens", "Mail") { }
			public override String Path => ">{path}>{token}";
		}

		public class Misc : Default
		{
			public Misc() : base("Settings", "Misc") { }
			public override String Path => "Misc";
		}

		public class Robots : Default
		{
			public Robots() : base("Generic", "Robots") { }
			public override String Path => "robots.txt";
		}

		public class SiteMap : Default
		{
			public SiteMap() : base("Generic", "SiteMap") { }
			public override String Path => "sitemap.txt";
		}
	}
}
