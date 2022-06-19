using System;
using System.Linq;
using System.Reflection;

namespace DFM.MVC.Helpers.Controllers
{
	public static class Wizard
	{
		public class Avoid : Attribute { }

		public static Boolean ShouldHaveWizard(this MemberInfo member)
		{
			return !member.GetCustomAttributes<Avoid>().Any();
		}
	}
}
