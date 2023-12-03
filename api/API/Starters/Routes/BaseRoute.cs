using System;
using JetBrains.Annotations;

namespace DFM.API.Starters.Routes
{
	public abstract class BaseRoute
	{
		protected BaseRoute() { }

		protected BaseRoute(
			[AspMvcController] String controller,
			[AspMvcAction] String action
		) : this()
		{
			Defaults = new {controller, action};
		}

		public abstract String Area { get; }
		public abstract String Path { get; }

		public Object Defaults { get; }

		public String Name => getName(GetType());

		private String getName(Type type)
		{
			return type.Name +
				(
					type.DeclaringType != null
						? "_" + getName(type.DeclaringType)
						: ""
				);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is BaseRoute @base && @base.Name == Name;
		}

		public override Int32 GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
