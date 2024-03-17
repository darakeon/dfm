using System;

namespace DFM.API.Starters.Routes
{
	public abstract class Apis : BaseRoute
	{
		public class Main : Apis
		{
			public override String Path => ObjectPath;
			public const String ObjectPath = "{controller=Tests}/{action=Index}";
		}

		public class Object : Apis
		{
			public override String Path => ObjectPath;
			public const String ObjectPath = "{controller=Tests}/{id?}/{action=Index}";
		}
	}
}
