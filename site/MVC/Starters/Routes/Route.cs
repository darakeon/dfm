using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DFM.MVC.Starters.Routes
{
	public class Route
	{
		internal const String DefaultArea = null;
		internal const String AccountArea = "Account";

		public static void Configure(IServiceCollection services)
		{
			services.AddControllersWithViews();
		}

		public static void CreateRoutes()
		{
			addUrl<Default.Robots>();
			addUrl<Default.SiteMap>();
			addUrl<Accounts>();
			addUrl<Default.Contract>();
			addUrl<Default.Misc>();
			addUrl<Default.Mail>();
			addUrl<Default.Mobile>();
			addUrl<Default.Main>();
		}

		private static void addUrl<T>()
			where T : BaseRoute, new()
		{
			var t = new T();
			var url = new Url(t.Path);
			urls.Add(t, url);
		}

		public static void Execute(IApplicationBuilder app)
		{
			app.Use<Route>("Routing", () => app.UseRouting());
			app.Use<Route>("Endpoints", () => app.UseEndpoints(mapAll));
		}

		private static void mapAll(IEndpointRouteBuilder endpoints)
		{
			urls.Keys.ToList()
				.ForEach(route => map(endpoints, route));
		}

		private static void map(IEndpointRouteBuilder endpoints, BaseRoute route)
		{
			if (route.Area == null)
				endpoints.MapControllerRoute(route.Name, route.Path, route.Defaults);
			else
				endpoints.MapAreaControllerRoute(route.Name, route.Area, route.Path, route.Defaults);
		}

		private static readonly IDictionary<BaseRoute, Url> urls
			= new Dictionary<BaseRoute, Url>();

		/// <summary>
		/// When you call this, don't forget to add the case
		/// to the tests, if it doesn't exist yet!
		/// </summary>
		/// <typeparam name="T">type of the route</typeparam>
		public static String GetUrl<T>(
			[AspMvcController] String controller,
			[AspMvcAction] String action,
			Object id = null
		) where T : BaseRoute, new()
		{
			return urls[new T()].Translate(new {controller, action, id});
		}
	}
}
