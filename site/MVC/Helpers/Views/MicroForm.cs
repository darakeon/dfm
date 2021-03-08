using System;
using System.Collections.Generic;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Starters.Routes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Helpers.Views
{
	public class MicroForm
	{
		private MicroForm(HttpContext context, String glyphicon, String text, String resource, String @class)
		{
			route = context.GetRouteText();

			Glyphicon = glyphicon;

			Text = text == null
				? resource
				: context.Translate(text);

			Class = @class;

			HiddenList = new Dictionary<String, object>();
		}

		public static MicroForm WithGlyph(HttpContext context, String glyphicon, String text)
		{
			return new MicroForm(context, glyphicon, text, null, null);
		}

		public static MicroForm WithText(HttpContext context, String text, String @class = null)
		{
			return new MicroForm(context, null, text, null, @class);
		}

		public static MicroForm WithResource(HttpContext context, String resource, String @class = null)
		{
			return new MicroForm(context, null, null, resource, @class);
		}

		private readonly Dictionary<String, String> route;

		public String Glyphicon { get; }
		public String Class { get; }
		public String Text { get; private set; }

		public String RouteName { get; private set; }
		public object RouteValues { get; private set; }

		public Boolean Ajax { get; private set; }
		public String UpdateTargetId { get; private set; }

		public IDictionary<String, object> HiddenList { get; }

		public MicroForm AddRouteIdUrl(
			String routeName,
			[AspMvcArea] String area,
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			object id
		)
		{
			RouteName = routeName;

			area ??= route["area"];
			controller ??= route["controller"];
			action ??= route["action"];

			RouteValues = new { area, controller, action, id };

			return this;
		}

		public MicroForm AddAccountIdUrl(
			String accountUrl,
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			object id
		)
		{
			var routeAccount = new Accounts();

			RouteName = routeAccount.Name;

			var area = routeAccount.Area;
			controller ??= route["controller"];
			action ??= route["action"];

			RouteValues = new { area, accountUrl, controller, action, id };

			return this;
		}

		public MicroForm AddRouteIdUrl<T>(
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			object id
		) where T : BaseRoute, new()
		{
			return AddRouteIdUrl(
				new T().Name,
				new T().Area,
				controller,
				action,
				id
			);
		}

		public MicroForm AddRouteUrl<T>(
			[AspMvcController] String controller,
			[AspMvcAction] String action
		) where T : BaseRoute, new()
		{
			AddRouteIdUrl<T>(controller, action, null);
			return this;
		}

		public MicroForm AddIdUrl(
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			object id
		)
		{
			AddRouteIdUrl(null, null, controller, action, id);
			return this;
		}

		public MicroForm AddIdUrl(
			[AspMvcAction] String action,
			object id
		)
		{
			AddIdUrl(null, action, id);
			return this;
		}

		public MicroForm AddHidden(String name, object value)
		{
			HiddenList.Add(name, value);
			return this;
		}

		public MicroForm AsAjax(String updateTargetId)
		{
			Ajax = true;
			UpdateTargetId = updateTargetId;
			return this;
		}

		public MicroForm EntityName(String entity)
		{
			Text += " " + entity;
			return this;
		}
	}
}
