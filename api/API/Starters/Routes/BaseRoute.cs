using System;

namespace DFM.API.Starters.Routes
{
	public abstract class BaseRoute
	{
		public abstract String Path { get; }

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
