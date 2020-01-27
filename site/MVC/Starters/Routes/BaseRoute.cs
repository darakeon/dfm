using System;

namespace DFM.MVC.Starters.Routes
{
	public abstract class BaseRoute
	{
		public abstract String Area { get; }
		public abstract String Path { get; }
		public virtual object Defaults => null;

		public String Name => GetType().Name;

		public override Boolean Equals(object obj)
		{
			return obj is BaseRoute @base && @base.Name == Name;
		}

		public override Int32 GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
