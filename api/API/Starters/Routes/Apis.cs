using System;

namespace DFM.API.Starters.Routes
{
	public abstract class Apis : BaseRoute
	{
		public const String ControllerPath = "{controller=Tests}";
		public const String IdPath = "{id?}";
		public const String ActionPath = "{action=Index}";
		public const String IdActionPath = $"{IdPath}/{ActionPath}";

		public class Main : Apis
		{
			public override String Path => ObjectPath;
			public const String ObjectPath = $"{ControllerPath}/{ActionPath}";
		}

		public class Object : Apis
		{
			public override String Path => ObjectPath;
			public const String ObjectPath = $"{ControllerPath}/{IdActionPath}";
		}
	}
}
