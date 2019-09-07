using System;
using DFM.MVC.Helpers.Global;
using Keon.MVC.Route;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers.Views
{
	public class MicroForm
	{
		private MicroForm(String glyphicon, String text, String resource, String @class)
		{
			Glyphicon = glyphicon;

			Text = text == null
				? resource
				: Translator.Dictionary[text];

			Class = @class;
		}

		public static MicroForm WithGlyph(String glyphicon, String text)
		{
			return new MicroForm(glyphicon, text, null, null);
		}

		public static MicroForm WithText(String text, String @class = null)
		{
			return new MicroForm(null, text, null, @class);
		}

		public static MicroForm WithResource(String resource, String @class = null)
		{
			return new MicroForm(null, null, resource, @class);
		}

		public String Glyphicon { get; }
		public String Text { get; }
		public String Class { get; }

		public String RouteName { get; private set; }
		public object RouteValues { get; private set; }

		public MicroForm AddRouteIdUrl(
			String route,
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			object id
		)
		{
			RouteName = route;

			var current = RouteInfo.Current.RouteData.Values;

			RouteValues = new
			{
				controller = controller ?? current["controller"],
				action = action ?? current["action"],
				id
			};
			return this;
		}

		public MicroForm AddRouteUrl(
			String route,
			[AspMvcController] String controller,
			[AspMvcAction] String action
		)
		{
			AddRouteIdUrl(route, controller, action, null);
			return this;
		}

		public MicroForm AddIdUrl(
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			object id
		)
		{
			AddRouteIdUrl(null, controller, action, id);
			return this;
		}

		public MicroForm AddUrl(
			[AspMvcController] String controller,
			[AspMvcAction] String action
		)
		{
			AddRouteIdUrl(null, controller, action, null);
			return this;
		}

		public MicroForm AddIdUrl(
			[AspMvcAction] String action,
			object id
		)
		{
			AddRouteIdUrl(null, null, action, id);
			return this;
		}

		public MicroForm AddUrl(
			[AspMvcAction] String action
		)
		{
			AddRouteIdUrl(null, null, action, null);
			return this;
		}
	}
}
