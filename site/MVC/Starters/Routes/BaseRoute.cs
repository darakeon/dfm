using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Starters.Routes
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

		public IHtmlContent MakeUrl(IHtmlHelper html,
			String text,
			[AspMvcAction] String action,
			[AspMvcController] String controller,
			Object htmlAttributes
		)
		{
			return html.RouteLink(
				text,
				Name,
				new {action, controller, area = Area},
				htmlAttributes
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
