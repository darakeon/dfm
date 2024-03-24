using System;

namespace DFM.API.Starters.Routes
{
	public abstract class Apis : BaseRoute
	{
		public const String ControllerPath = "{controller=Tests}";

		public class Main : Apis
		{
			public override String Path => ObjectPath;
			public const String ObjectPath = ControllerPath + "/{action=Index}";
		}

		public class Object : Apis
		{
			public override String Path => ObjectPath;
			public const String ObjectPath = ControllerPath + "/{id?}/{action=Index}";
		}
	}
}
