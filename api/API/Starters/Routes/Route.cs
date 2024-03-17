using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DFM.API.Starters.Routes
{
	public class Route
	{
		public static void Configure(IServiceCollection services)
		{
			services.AddControllersWithViews();
		}

		public static void CreateRoutes()
		{
			addUrl<Apis.Main>();
			addUrl<Apis.Object>();
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
				.ForEach(
					route => endpoints.MapControllerRoute(route.Name, route.Path)
				);
		}

		private static readonly IDictionary<BaseRoute, Url> urls
			= new Dictionary<BaseRoute, Url>();
	}
}
